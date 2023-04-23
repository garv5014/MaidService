using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using Supabase.Interfaces;
using Supabase.Storage;
using System.Formats.Tar;

namespace MaidService.Services;

public class MaidStorage : ISupabaseStorage
{
    private IPlatformService _platformService;
    private Supabase.Client _client;
    private readonly IAuthService auth;

    public MaidStorage(IPlatformService platform, Supabase.Client client, IAuthService auth)
    {
        _platformService = platform;
        _client = client;
        this.auth = auth;
    }
    private async Task updateUserTableWithNewUrl(string url)
    {
        var role = await auth.GetUserRole();
        var publicUrl = _client
                           .Storage
                           .From("profile-pictures")
                           .GetPublicUrl(url);
        if (role == "Cleaner")
        {
            await _client.From<CleanerModel>()
                  .Where(x => x.AuthId == _client.Auth.CurrentUser.Id)
                  .Set(x => x.ProfilePicture, publicUrl)
                  .Update();
        }
        else if (role == "Customer")
        {
            await _client.From<CustomerModel>()
                  .Where(x => x.AuthId == _client.Auth.CurrentUser.Id)
                  .Set(x => x.ProfilePicture, publicUrl)
                  .Update();
        }
        else
        {
            throw new Exception("User role not found");
        }
    }

    public async Task UploadProfilePicture()
    {
        string photoDevicePath = await pickAPhotoFromFileSystem();
        string fileNameForSupabase = getProfileHash();
        if (!string.IsNullOrEmpty(photoDevicePath))
        {
            try
            {
                await storeProfilePictureInBucket(photoDevicePath, fileNameForSupabase);
                await updateUserTableWithNewUrl(fileNameForSupabase);
            }
            catch (BadRequestException e)
            {
                if (e.ErrorResponse.Error == "Duplicate")
                    await replaceProfilePictureInBucket(photoDevicePath, fileNameForSupabase);
                await updateUserTableWithNewUrl(fileNameForSupabase);
            }
            _platformService.DisplayAlert("Success", "Profile picture updated, Please Refresh your cache if it doesn't appear to update", "OK");
        }
    }
    public IEnumerable<Cleaner> GetCleanersProfilePicturesFromAContract(CleaningContract contract)
    {
        var cleaners = new List<Cleaner>();
        foreach (var cleaner in contract.AvailableCleaners)
        {
            var pfpHash = getProfileHash(cleaner.Cleaner);
            var cleanerProfilePicture = _client.Storage
                .From("profile-pictures")
                .GetPublicUrl(pfpHash);
            cleaner.Cleaner.ProfilePicture = cleanerProfilePicture;
            cleaners.Add(cleaner.Cleaner);
        }
        return cleaners;
    }
    private async Task storeProfilePictureInBucket(string photoToBeUploadedPath, string fileNameForSupabase)
    {
        var res = await _client.Storage
                      .From("profile-pictures")
                      .Upload(photoToBeUploadedPath, fileNameForSupabase);
    }
    private async Task<string> pickAPhotoFromFileSystem()
    {
        var res = await _platformService.PickImageFile(PickOptions.Images);
        var photoUrl = res?.FullPath;
        return photoUrl;
    }
    private async Task replaceProfilePictureInBucket(string photoToBeUploadedPath, string fileNameForSupabase)
    {
        var res = await _client.Storage
                    .From("profile-pictures")
                    .Remove(new List<string> { fileNameForSupabase });
        await storeProfilePictureInBucket(photoToBeUploadedPath, fileNameForSupabase);
    }
    private string getProfileHash(PublicUser targetUser = null)
    {
        string supabaseUrl = $"{Guid.NewGuid()}.jpg";
        return supabaseUrl;
    }
}
