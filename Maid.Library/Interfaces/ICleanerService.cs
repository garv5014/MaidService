using MaidService.Library.DbModels;

namespace Maid.Library.Interfaces;

public interface ICleanerService
{
    Task<Cleaner> GetCurrentCleaner();
    Task AddNewCleaner(string firstName, string lastName, string phoneNumber, string userEmail,string authId);
    Task UpdateCleanerBio(string bioText);
    Task<IEnumerable<CleaningContract>> GetAllAvailableAppointments();
    Task<IEnumerable<Schedule>> GetSchedulesForADate(DateTime scheduleDate);
}
