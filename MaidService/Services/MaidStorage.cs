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
    public string GetProfilePictureFromSupabase()
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
    private async Task updateUserTableWithNewUrl()
    {
        var url = GetProfilePictureFromSupabase();
        var role = await auth.GetUserRole();
        if (role == "Cleaner")
        {
            await _client.From<CleanerModel>()
                  .Where(x => x.AuthId == _client.Auth.CurrentUser.Id)
                  .Set(x => x.ProfilePicture, url)
                  .Update();
        }
        else if (role == "Customer")
        {
            await _client.From<CustomerModel>()
                  .Where(x => x.AuthId == _client.Auth.CurrentUser.Id)
                  .Set(x => x.ProfilePicture, url)
                  .Update();
        }
        else { 
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
            await updateUserTableWithNewUrl();
        }
        catch (BadRequestException e)
        {
            if (e.ErrorResponse.Error == "Duplicate")
                await replaceProfilePictureInBucket(photoDevicePath, fileNameForSupabase);
                await updateUserTableWithNewUrl();
            }
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
    await _client.Storage
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
    await _client.Storage
        .From("profile-pictures")
        .Remove(new List<string> { fileNameForSupabase });
    await storeProfilePictureInBucket(photoToBeUploadedPath, fileNameForSupabase);
}
private string getProfileHash(PublicUser targetUser = null)
{
    if (targetUser == null)
    {
        var currentUser = _client.Auth.CurrentUser;
        string supabaseUrl = "profile_picture_" + currentUser.Id[..5];
        return supabaseUrl;
    }
    else if (targetUser.GetType() == typeof(Cleaner) || targetUser.GetType() == typeof(Customer))
    {
        return "profile_picture_" + targetUser.AuthId[..5];
    }
    throw new Exception("profile hash not found");
}
}
