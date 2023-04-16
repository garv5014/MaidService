

using MaidService.Library.DbModels;

namespace Maid.Library.Interfaces;

public interface ISupabaseStorage
{
    Task UploadProfilePicture();
    string GetProfilePictureFromSupabase();
    IEnumerable<Cleaner> GetCleanersProfilePicturesFromAContract(CleaningContract cardContract);
}
