﻿using AutoMapper;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using Postgrest;
using Postgrest.Responses;
using Supabase;
using Supabase.Storage;
using System.Globalization;
using static Postgrest.Constants;

namespace MaidService.Services;

public class CustomerService : ICustomerService
{
    private readonly Supabase.Client _client;
    private readonly IMapper _mapper;
    private readonly IAuthService _auth;

    public CustomerService(Supabase.Client client, IMapper mapper, IAuthService auth)
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
                        string authId)
    {
        userEmail = userEmail.Trim();
        firstName = firstName.Trim();
        lastName = lastName.Trim();
        phoneNumber = phoneNumber.Trim();
        authId = authId.Trim();
        var model = new CustomerModel
        {
            FirstName = firstName,
            SurName = lastName,
            PhoneNumber = phoneNumber,
            Email = userEmail,
            AuthId = authId
        };
        await _client.From<CustomerModel>().Insert(model);
    }

    public async Task<IEnumerable<CleaningContract>> GetAllAppointments()
    {
        var cust = await GetCurrentCustomer();
        var result =
            await _client.From<CleaningContractModel>()
            .Filter("cust_id", Operator.Equals, cust.Id)
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

        return isResponseSucessfulAndPopulated(contract)
            ? _mapper.Map<CleaningContract>(contract.Models?.First())
            : new CleaningContract();
    }

    private bool isResponseSucessfulAndPopulated(ModeledResponse<CleaningContractModel> contract)
    {
        return contract.ResponseMessage.IsSuccessStatusCode
                    && contract.Models.Count > 0;
    }

    public async Task<IEnumerable<CleaningContract>> GetUpcomingAppointments(int customerId)
    {
        var yesterday = DateTime.Now - TimeSpan.FromDays(1);
        var result = await _client.Postgrest
            .Table<CleaningContractModel>()
            .Where(c => c.Customer_Id == customerId && c.ScheduleDate > yesterday)
            .Get();

        return isResponseSucessfulAndPopulated(result)
            ? _mapper.Map<List<CleaningContract>>(result.Models)
            : new List<CleaningContract>();
    }

    public async Task<bool> IsScheduled(int contractId)
    {
        var res = await _client
        .From<CleanerAssignmentModel>()
         .Where(c => c.Contract_Id == contractId)
         .Limit(1)
         .Get();

        return res.Models?.Count != 0;
    }

    public async Task<Customer> GetCurrentCustomer()
    {
        CustomerModel customer = await queryForCurrentCustomer();
        return customer?.AuthId != null
            ? _mapper.Map<Customer>(customer)
            : null;
    }

    private async Task<CustomerModel> queryForCurrentCustomer()
    {
        var user = _auth.GetCurrentUser();
        var customer = await _client
            .From<CustomerModel>()
            .Filter("auth_id", Operator.Equals, user.Id)
            .Single();

        return customer;
    }

    public async Task CreateNewContract(CleaningContract contract, List<string> photosToUpload = null)
    {
        var location = new LocationModel()
        {
            Address = contract.Location.Address,
            City = contract.Location.City,
            State = contract.Location.State,
            ZipCode = contract.Location.ZipCode
        };
        var locationResult = await _client
            .From<LocationModel>()
            .Insert(location);

        var customer = await GetCurrentCustomer();
        var contractModel = new CleaningContractModelNoReferences
        {
            RequestedHours = contract.RequestedHours,
            Customer_Id = customer.Id,
            ScheduleDate = contract.ScheduleDate,
            EstSqft = contract.EstSqft,
            LocationId = locationResult.Models.First().Id,
            Notes = contract.Notes,
            CleaningTypeId = contract.CleaningType.Id,
            Cost = contract.Cost,
            NumOfCleaners = 1,  //needs at least one cleaner 
            DateCompleted = null
        };

        try
        {
            var result = await _client
                .From<CleaningContractModelNoReferences>()
                .Insert(contractModel);
            await uploadHousePhotos(photosToUpload, result.Models.First().Id);
            result.ResponseMessage.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {

        }
    }

    private async Task uploadHousePhotos(List<string> photosToUpload, int contractId)
    {
        try
        {
            foreach (var photo in photosToUpload)
            {
                var fileName = $"{contractId}_{Guid.NewGuid()}.jpg";
                var res = await _client.Storage
                          .From("contract-photos")
                          .Upload(photo, fileName);

                var publicUrl = _client.Storage
                    .From("contract-photos")
                    .GetPublicUrl(fileName);

                var resContractPhotos = _client
                    .From<ContractPhotoModel>()
                    .Insert(new ContractPhotoModel { ContractId = contractId, PhotoUrl = publicUrl });

            }
        }
        catch (Exception e)
        {

        }
    }
}
