using AutoMapper;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using Supabase;
using static Postgrest.Constants;

namespace MaidService.Services;

public class CustomerService : ICustomerService
{
    private readonly Client _client;
    private readonly IMapper _mapper;

    public CustomerService(Supabase.Client client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CleaningContract>> GetAllAppointments(int customerId)
    {
        var result =
            await _client.From<CleaningContractModel>()
            .Select("*")
            .Filter("cust_id", Operator.Equals, customerId)
            .Get();

        return result.Models.Count > 0
            ? _mapper.Map<IEnumerable<CleaningContract>>(result.Models)
            : null; 
    }

    public async Task<CleaningContract> GetCleaningDetailsById(int contractId)
    {
        var contract = await _client
            .From<CleaningContractModel>()
            .Where(c => c.Id == contractId)
            .Limit(1)
            .Get();
        var cleaners = await _client
            .From<CleaningContractModel>()
            .Where (c => c.Id == contractId)
            .Get();

        return contract.ResponseMessage.IsSuccessStatusCode
            ? _mapper.Map<CleaningContract>(contract.Models[0])
            : new CleaningContract();
    }

    public async Task<IEnumerable<CleaningContract>> GetUpcomingAppointments(int customerId)
    {
        var res = await _client
            .From<CleaningContractModel>()
            .Where(c => c.ScheduleDate > (DateTime.Now))
            //.Where(c => c.ScheduleDate < (DateTime.Now + TimeSpan.FromDays(8)))
            //.Where(c => c.Id == customerId)
            .Get();
        return res.Models.Count > 0
            ? _mapper.Map<List<CleaningContract>>(res.Models)
            : null;
    }
}
