

using MaidService.Library.DbModels;

namespace Maid.Library.Interfaces;

public interface ISupabaseStorage
{
    Task UploadProfilePicture();
    IEnumerable<Cleaner> GetCleanersProfilePicturesFromAContract(CleaningContract cardContract);
}
