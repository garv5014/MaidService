using AutoMapper;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using Supabase;

namespace MaidService.Services;

public class CustomerService : ICustomerService
{
    private readonly Client _client;
    private readonly IMapper _mapper;

    CustomerService(Supabase.Client client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task<List<CleaningContract>> GetUpcomingAppointments()
    {
        var res = await _client.From<CleaningContractModel>().Get();
        return  res.Models.Count > 0 
            ? _mapper.Map<List<CleaningContract>>(res.Models)
            : null;
    }
}
