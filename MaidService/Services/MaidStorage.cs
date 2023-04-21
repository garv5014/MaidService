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

    public MaidStorage(IPlatformService platform, Supabase.Client client)
    {
        _platformService = platform;
        _client = client;
    }
    public string GetProfilePictureFromSupabase()
    {
        var profilePicture = getProfileHash();
        string publicUrl = "";
        try
        {
            publicUrl =  _client
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
        if (!string.IsNullOrEmpty(photoDevicePath))
        { 
            try
            { await storeProfilePictureInBucket(photoDevicePath, fileNameForSupabase); }
            catch (BadRequestException e)
            {
                if (e.ErrorResponse.Error == "Duplicate")
                    await replaceProfilePictureInBucket(photoDevicePath, fileNameForSupabase);
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
            cleaner.Cleaner.ProfilePictureUrl = cleanerProfilePicture;
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
        var res = await _platformService.PickFile(PickOptions.Images);
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
