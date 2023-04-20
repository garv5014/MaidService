using AutoMapper;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using Newtonsoft.Json;
using System.Reactive.Concurrency;
using static Postgrest.Constants;

namespace MaidService.Services;

public class CleanerService : ICleanerService
{
    private readonly Supabase.Client _client;
    private readonly IMapper _mapper;
    private readonly IAuthService _auth;

    public CleanerService(Supabase.Client client, IMapper mapper, IAuthService auth)
    {
        _client = client;
        _mapper = mapper;
        _auth = auth;
    }

    public async Task AddNewCleaner(
        string firstName
        , string lastName
        , string phoneNumber
        , string userEmail
        , string authId)
    {
        userEmail = userEmail.Trim();
        firstName = firstName.Trim();
        lastName = lastName.Trim();
        phoneNumber = phoneNumber.Trim();
        authId = authId.Trim();
        var model = new CleanerModel
        {
            FirstName = firstName,
            SurName = lastName,
            PhoneNumber = phoneNumber,
            Email = userEmail,
            AuthId = authId
        };
        await _client.From<CleanerModel>().Insert(model);
    }

    public async Task<IEnumerable<CleaningContract>> GetAllAvailableAppointments()
    {
        var yesterday = DateTime.Now.AddDays(-1);
        var allContracts = await _client.From<CleaningContractModel>()
                   .Where(c => c.ScheduleDate > yesterday)
                   .Get();

        var availableAppointments = allContracts.Models.Where(x =>
          x.AssingedCleaners.Count < x.NumOfCleaners
        ).ToList();
        return _mapper.Map<IEnumerable<CleaningContract>>(availableAppointments);
    }

    public async Task<Cleaner> GetCurrentCleaner()
    {
        CleanerModel cleaner = await queryForCurrentCleaner();
        return cleaner?.AuthId != null
            ? _mapper.Map<Cleaner>(cleaner)
            : null;
    }

    private async Task<CleanerModel> queryForCurrentCleaner()
    {
        var user = _auth.GetCurrentUser();

        var cleaner = await _client
            .From<CleanerModel>()
            .Filter("auth_id", Operator.Equals, user.Id)
            .Single();
        return cleaner;
    }

    public async Task UpdateCleanerBio(string bioText)
    {
        var cleaner = await GetCurrentCleaner();

        var update = await _client
                       .From<CleanerModel>()
                       .Where(x => x.AuthId == cleaner.AuthId)
                       .Set(x => x.Bio, bioText)
                       .Update();
    }

    public async Task<IEnumerable<Schedule>> GetAvailableSchedulesForADate(DateTime scheduleDate)
    {
        var schedulesModels = await _client.From<ScheduleModel>()
            .Where(ca => ca.Date == scheduleDate)
            .Get();
        var schedules = _mapper.Map<IEnumerable<Schedule>>(schedulesModels.Models).ToList();

        List<CleanerAvailabilitySchedule> cleanerAvailabileSchedules = await getAllCleanerAvalbilityForCurrentCleaner();

        var availableSchedules = new List<Schedule>();
        foreach (var schedule in cleanerAvailabileSchedules)
        {
            availableSchedules.Add(schedule.Schedule);
        }

        var comparer = new ScheduleEqualityComparer();
        availableSchedules = schedules.Except(availableSchedules, comparer).ToList();
        return availableSchedules;
    }

    public async Task<IEnumerable<CleaningContractWithStartTime>> GetAllScheduledAppointmentsForAWeek(DateTime startDate)
    {
        var cleaner = await GetCurrentCleaner();
        var response = await _client.Rpc("getallassignedslotsforacleanerforaweekfromdate", new Dictionary<string, object> { { "target_cleaner_id", cleaner.Id }, { "target_date", startDate } });
        
        var deserializedResponse = JsonConvert.DeserializeObject<List<CleaningContractWithStartTime>>(response.Content);
        

        
        return deserializedResponse;
    }

    private async Task<List<CleanerAvailabilitySchedule>> getAllCleanerAvalbilityForCurrentCleaner()
    {
        var cleaner = await GetCurrentCleaner();
        var cleanerAvailability = await _client.From<CleanerAvailabilityModel>()
            .Select("schedule_id")
            .Where(ca => ca.Cleaner_Id == cleaner.Id)
            .Get();

        var cleanerAvailabileSchedules = _mapper.Map<IEnumerable<CleanerAvailabilitySchedule>>(cleanerAvailability.Models).ToList();
        return cleanerAvailabileSchedules;
    }


    public async Task UpdateCleanerAvailablility(IEnumerable<object> newAvailability)
    {

        var cleaner = await GetCurrentCleaner();
        var selectedSlots = newAvailability.ToList();
        foreach (var item in selectedSlots)
        {
            await _client.From<CleanerAvailabilityModel>()
                .Insert(new CleanerAvailabilityModel
                {
                    Schedule_Id = _mapper.Map<ScheduleModel>(item).Id,
                    Cleaner_Id = cleaner.Id,
                });
        }
    }

    public async Task<IEnumerable<Schedule>> GetCleanerAvailabilityForASpecificContract(CleaningContract contract)
    {
        var availableFilteredTimes = new List<Schedule>();
        var cleaner = await GetCurrentCleaner();
        var result = await _client.Rpc("getavailableslotsforacontract", new Dictionary<string, object>
        {
            { "contract_id" , contract.Id }
            , { "target_cleaner_id", cleaner.Id }
        });
        // deserialize result.Content to get the list of available times
        var allAvailableTimes = JsonConvert.DeserializeObject<List<Schedule>>(result.Content);
        foreach (var item in allAvailableTimes)
        {
            var hasEnoughSlots = allAvailableTimes.Where(c => c.StartTime.TimeOfDay <= (item.StartTime.TimeOfDay + contract.RequestedHours) && c.StartTime.TimeOfDay >= item.StartTime.TimeOfDay).ToList();
            var sum = hasEnoughSlots.Sum(x => x.Duration.Hours);
            if (sum >= contract.RequestedHours.Hours)
            {
                availableFilteredTimes.Add(item);
            }
        }

        availableFilteredTimes = availableFilteredTimes.OrderBy(c => c.StartTime).ToList();

        return availableFilteredTimes;
    }

    public async Task UpdateCleanerAssignments(CleaningContract contract, Schedule schedule)
    {
        // get all the schedules that are within the requested hours
        var cleaner = await GetCurrentCleaner();
        var upperbound = schedule.StartTime + contract.RequestedHours;
        var schedulesToBeAdded = await _client.From<ScheduleModel>()
            .Where(s => s.Date == contract.ScheduleDate)
            .Where(s => s.StartTime >= schedule.StartTime)
            .Where(s => s.StartTime < upperbound)
            .Get();
        var cleanerAvailabilityListIds = new List<CleanerAvailabilityModel>(); 

        foreach (var item in schedulesToBeAdded.Models)
        {
            var clearAvailibilityItem = await _client.From<CleanerAvailabilityModel>()
                .Where(ca => ca.Cleaner_Id == cleaner.Id)
                .Where(ca => ca.Schedule_Id == item.Id)
                .Single();
            cleanerAvailabilityListIds.Add(clearAvailibilityItem);
        }
        foreach (var item in cleanerAvailabilityListIds)
        {
            var cass = new CleanerAssignmentModel
            {
                Cleaner_Availability_Id = item.Id,
                Contract_Id = contract.Id
            };
            await _client.From<CleanerAssignmentModel>()
                .Insert(cass);
        }

        var newScheduledDate = new DateTime(year: contract.ScheduleDate.Year,
            month: contract.ScheduleDate.Month,
            day: contract.ScheduleDate.Day,
            hour: schedule.StartTime.Hour,
            minute: schedule.StartTime.Minute,
            second: 0);

        await _client.From<CleaningContractModel>()
                       .Where(x => x.Id == contract.Id)
                       .Set(x => x.ScheduleDate, newScheduledDate)
                       .Update();
    }

    public async Task<IEnumerable<CleaningContractWithStartTime>> GetUpcomingAppointments()
    {
        var cleaner = await GetCurrentCleaner();

        var response = await _client.Rpc("getallassignedslotsforacleaner", new Dictionary<string, object> { { "target_cleaner_id", cleaner.Id} });
        var result = JsonConvert.DeserializeObject<List<CleaningContractWithStartTime>>(response.Content);

        return result;
    }

    public async Task<CleaningContract> GetCleaningContractDetails(int contractId)
    {
        var result = await _client.From<CleaningContractModel>()
                                  .Where(cc => cc.Id == contractId)
                                  .Single();

        return _mapper.Map<CleaningContract>(result);
    }

}
