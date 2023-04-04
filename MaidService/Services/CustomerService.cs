﻿using AutoMapper;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using Supabase;
using System.Diagnostics.Contracts;
using static Postgrest.Constants;

namespace MaidService.Services;

public class CustomerService : ICustomerService
{
    private readonly Client _client;
    private readonly IMapper _mapper;
    private readonly IAuthService _auth;

    public CustomerService(Client client, IMapper mapper, IAuthService auth)
    {
        _client = client;
        _mapper = mapper;
        _auth = auth;
    }

    public async Task<IEnumerable<CleaningType>> GetCleaningTypes()
    {
        var result = await _client.From<CleaningTypeModel>().Get();

        return result.Models.Count > 0
            ? _mapper.Map<IEnumerable<CleaningType>>(result.Models)
            : null;
    }

    public async Task AddCustomer(string firstName,
                        string lastName,
                        string phoneNumber,
                        string userEmail,
                        string AuthId)
    {
        userEmail = userEmail.Trim();
        firstName = firstName.Trim();
        lastName = lastName.Trim();
        phoneNumber = phoneNumber.Trim();
        AuthId = AuthId.Trim();
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
            await _client.From<CleaningContractModelNoCleaners>()
            .Filter("cust_id", Operator.Equals, customerId)
            .Get();

        return result?.Models?.Count > 0
            ? _mapper.Map<IEnumerable<CleaningContract>>(result.Models)
            : new List<CleaningContract>();
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
            : new List<CleaningContract>();
    }

    public async Task<bool> IsScheduled(int contractId)
    {
        var res = await _client
        .From<CleaningContractModel>()
         .Where(c => c.Id == contractId)
         .Limit(1)
         .Get();

        return res.Models?.Count == 0;
    }

    public async Task<Customer> GetCurrentCustomer()
    {
        var user = _auth.GetCurrentUser();

        var cust = await _client
            .From<CustomerModel>()
            .Filter("auth_id", Operator.Equals, user.Id)
            .Single();
        return cust.AuthId != null
            ? _mapper.Map<Customer>(cust)
            : null;
    }

    public async Task CreateNewContract(CleaningContract contract)
    {
        var cust = await GetCurrentCustomer();
        var contractModel = new CleaningContractModel
        {
            RequestedHours = contract.RequestedHours,
            Customer_Id =  cust.Id,
            ScheduleDate = contract.ScheduleDate,
            EstSqft = contract.EstSqft,
            Location = new LocationModel()
            {
                Address = contract.Location.Address,
                City = contract.Location.City,
                State = contract.Location.State,
                ZipCode = contract.Location.ZipCode

            },
            Notes = contract.Notes,
            CleaningTypeId = contract.CleaningType.Id
        };
        try
        {
            var res = await _client
                .From<CleaningContractModel>()
                .Insert(contractModel);
            res.ResponseMessage.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {             
            
        }
    }
}
