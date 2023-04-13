using MaidService.Library.DbModels;

namespace Maid.Library.Interfaces;

public interface ICleanerService
{
    Task<Cleaner> GetCurrentCleaner();
    Task AddNewCleaner(string firstName, string lastName, string phoneNumber, string userEmail,string authId);
    Task UpdateCleanerBio(string bioText);
    Task UploadProfilePicture(int retries = 0);
    Task<string> GetProfilePicturePath();
    Task<IEnumerable<CleaningContract>> GetAllAvailableAppointments();
}
