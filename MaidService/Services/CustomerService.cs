using AutoMapper;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using Supabase;

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

    public async Task<List<CleaningContract>> GetUpcomingAppointments()
    {
        var res = await _client.From<CleaningContractModel>().Get();
        return res.Models.Count > 0
            ? _mapper.Map<List<CleaningContract>>(res.Models)
            : null;
    }
}
