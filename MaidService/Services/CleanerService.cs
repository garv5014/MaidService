using AutoMapper;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.ViewModels;
using Newtonsoft.Json;
using Postgrest.Interfaces;
using System.Linq;
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
        ,string lastName
        ,string phoneNumber
        ,string userEmail
        ,string authId)
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
        return cleaner.AuthId != null
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
        var availableSchedules = new List<Schedule>();
        var cleaner = await GetCurrentCleaner();
        var schedulesModels = await _client.From<ScheduleModel>()
            .Where(ca => ca.Date == scheduleDate)
            .Get();
        var schedules = _mapper.Map<IEnumerable<Schedule>>(schedulesModels.Models).ToList();

        var cleanerAvailability = await _client.From<CleanerAvailabilityModel>()
            .Where(ca => ca.Cleaner_Id == cleaner.Id)
            .Get();

        var cleanerAvailabileSchedules = _mapper.Map<IEnumerable<CleanerAvailabilitySchedule>>(cleanerAvailability.Models).ToList();

        foreach (var schedule in cleanerAvailabileSchedules)
        {
            availableSchedules.Add(schedule.Schedule);
        }
        var comparer = new ScheduleEqualityComparer();
        availableSchedules = schedules.Except(availableSchedules, comparer).ToList();
        return availableSchedules;

    }

    public async Task UpdateCleanerAvailablility(IEnumerable<object> newAvailability)
    {

        var cleaner = await GetCurrentCleaner();
        var selectedSlots = newAvailability.ToList();
        foreach (var item in selectedSlots)
        {
            await _client.From<CleanerAvailabilityModel>()
                .Insert( new CleanerAvailabilityModel
                {
                    Schedule_Id = _mapper.Map<ScheduleModel>(item).Id,
                    Cleaner_Id = cleaner.Id,
                });
        }
    }

    public async Task<IEnumerable<Schedule>> GetCleanerAvailabilityForASpecificContract(CleaningContract contract)
    {
        var availableTimes = new List<Schedule>();
        var cleaner = await GetCurrentCleaner();
        var result = await _client.Rpc("getavailableslotsforacontract", new Dictionary<string, object>
        {
            { "contract_id" , contract.Id }
            , { "target_cleaner_id", cleaner.Id } 
        });
        // deserialize result.Content to get the list of available times
        availableTimes = JsonConvert.DeserializeObject<List<Schedule>>(result.Content);
        return availableTimes;
    }

    public async Task UpdateCleanerAssignments(int contractId, object newAssignment)
    {
        await _client.From<CleanerAssignmentModel>()
            .Insert(new CleanerAssignmentModel
            {
                Contract_Id = contractId,
                Cleaner_Availability_Id = _mapper.Map<CleanerAssignmentModel>(newAssignment).Id,
            });
    }
}
