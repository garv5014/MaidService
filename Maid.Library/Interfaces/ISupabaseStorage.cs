

namespace Maid.Library.Interfaces;

public interface ISupabaseStorage
{
    Task UploadProfilePicture();
    Task<string> GetProfilePictureFromSupabase();

}
