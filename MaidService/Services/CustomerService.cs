﻿using AutoMapper;
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

    public async Task AddCustomer(string firstName, string lastName, string phoneNumber, string userEmail, string AuthId)
    {
        var model = new CustomerModel
        {
            FirstName = firstName,
            SurName = lastName,
            PhoneNumber = phoneNumber,
            Email = userEmail,
            AuthId = AuthId
        };
        await _client.From<CustomerModel>().Insert(model);
    }

    public async Task<IEnumerable<CleaningContract>> GetAllAppointments(int customerId)
    {
        var result =
            await _client.From<CleaningContractModel>()
            .Where(c => c.Customer.Id == customerId)
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

        return contract.ResponseMessage.IsSuccessStatusCode
            ? _mapper.Map<CleaningContract>(contract.Models[0])
            : new CleaningContract();
    }

    public async Task<IEnumerable<CleaningContract>> GetUpcomingAppointments(int customerId)
    {
        var res = await _client.Postgrest
            .Table<CleaningContractModel>()
            .Where(c => c.Customer_Id == customerId)
            .Get();
        return res.Models.Count > 0
            ? _mapper.Map<List<CleaningContract>>(res.Models)
            : null;
    }
}
