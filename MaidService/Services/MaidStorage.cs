using Maid.Library.Interfaces;
using Supabase.Interfaces;
using Supabase.Storage;

namespace MaidService.Services;

public class MaidStorage : ISupabaseStorage
{
    private IPlatformService _platformService;
    private Supabase.Client _client;

    public MaidStorage(IPlatformService platform, Supabase.Client client)
    {
        _platformService = platform;
        _client = client;
    }
    public async Task<string> GetProfilePictureFromSupabase()
    {
        var profilePicture = getProfileHash();
        string publicUrl = "";
        try
        {
            publicUrl = _client
                            .Storage
                            .From("profile-pictures")
                            .GetPublicUrl(profilePicture);
        }
        catch (Exception e)
        {
            //
        }
        return publicUrl;
    }

    public async Task UploadProfilePicture()
    {
        string photoDevicePath = await pickAPhotoFromFileSystem();
        string fileNameForSupabase = getProfileHash();
        try
        { await storeProfilePictureInBucket(photoDevicePath, fileNameForSupabase); }
        catch (BadRequestException e)
        {
            if (e.ErrorResponse.Error == "Duplicate")
                await replaceProfilePictureInBucket(photoDevicePath, fileNameForSupabase);
        }
    }
    private async Task storeProfilePictureInBucket(string photoToBeUploadedPath, string fileNameForSupabase)
    {
        await _client.Storage
                      .From("profile-pictures")
                      .Upload(photoToBeUploadedPath, fileNameForSupabase);
    }
    private async Task<string> pickAPhotoFromFileSystem()
    {
        var res = await _platformService.PickFile();
        var photoUrl = res.FullPath;
        return photoUrl;
    }
    private async Task replaceProfilePictureInBucket(string photoToBeUploadedPath, string fileNameForSupabase)
    {
        await _client.Storage
            .From("profile-pictures")
            .Remove(new List<string> { fileNameForSupabase });
        await storeProfilePictureInBucket(photoToBeUploadedPath, fileNameForSupabase);
    }
    private string getProfileHash()
    {
        var cust = _client.Auth.CurrentUser;
        string supabaseUrl = "profile_picture_" + cust.Id[..5];
        return supabaseUrl;
    }

}
