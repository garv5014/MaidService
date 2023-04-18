using MaidService.Library.DbModels;
using System.Diagnostics.Contracts;

namespace Maid.Library.Interfaces;

public interface ICleanerService
{
    Task<Cleaner> GetCurrentCleaner();
    Task AddNewCleaner(string firstName, string lastName, string phoneNumber, string userEmail,string authId);
    Task UpdateCleanerBio(string bioText);
    Task<IEnumerable<CleaningContract>> GetAllAvailableAppointments();
    Task<IEnumerable<Schedule>> GetAvailableSchedulesForADate(DateTime scheduleDate);
    Task UpdateCleanerAvailablility(IEnumerable<object> selectedSlots);
    Task<IEnumerable<Schedule>> GetCleanerAvailabilityForASpecificContract(CleaningContract contract);
    Task UpdateCleanerAssignments(int contractId, Schedule scheduleId);
}
