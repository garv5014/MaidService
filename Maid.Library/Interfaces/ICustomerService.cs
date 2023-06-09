﻿using MaidService.Library.DbModels;

namespace Maid.Library.Interfaces;

public interface ICustomerService
{
    Task<CleaningContract> GetCleaningDetailsById(int contractId);
    Task<IEnumerable<CleaningContract>> GetUpcomingAppointments(int customerId);
    Task<IEnumerable<CleaningContract>> GetAllAppointments();
    Task<IEnumerable<CleaningType>> GetCleaningTypes();
    Task<bool> IsScheduled(int contractId);
    Task AddCustomer(string firstName, string lastName, string phoneNumber, string userEmail, string AuthId);
    Task<Customer> GetCurrentCustomer();
    Task CreateNewContract(CleaningContract contract, List<string> photosToUpload = null);
}
