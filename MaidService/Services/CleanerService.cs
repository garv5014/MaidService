using AutoMapper;
using Maid.Library.Interfaces;
using MaidService.ComponentsViewModels;
using MaidService.Library.DbModels;
using Supabase.Storage;
using static Postgrest.Constants;

namespace MaidService.Services;

public class CleanerService : ICleanerService
{
    private readonly Supabase.Client _client;
    private readonly IMapper _mapper;
    private readonly IAuthService _auth;
    private readonly IPlatformService _platformService;

    public CleanerService(Supabase.Client client, IMapper mapper, IAuthService auth, IPlatformService platformService)
    {
        _client = client;
        _mapper = mapper;
        _auth = auth;
        _platformService = platformService;
    }

    public async Task AddNewCleaner(
        string firstName
        ,string lastName
        ,string phoneNumber
        ,string userEmail
        ,string authId)
    {
        userEmail = userEmail.Trim();
        firstName = firstName.Trim();
        lastName = lastName.Trim();
        phoneNumber = phoneNumber.Trim();
        authId = authId.Trim();
        var model = new CleanerModel
        {
            FirstName = firstName,
            SurName = lastName,
            PhoneNumber = phoneNumber,
            Email = userEmail,
            AuthId = authId
        };
        await _client.From<CleanerModel>().Insert(model);
    }

    public async Task<IEnumerable<CleaningContract>> GetAllAvailableAppointments()
    {
       throw new NotImplementedException();
    }

    public async Task<Cleaner> GetCurrentCleaner()
    {
        CleanerModel cleaner = await queryForCurrentCleaner();
        return cleaner.AuthId != null
            ? _mapper.Map<Cleaner>(cleaner)
            : null;
    }

    private async Task<CleanerModel> queryForCurrentCleaner()
    {
        var user = _auth.GetCurrentUser();

        var cleaner = await _client
            .From<CleanerModel>()
            .Filter("auth_id", Operator.Equals, user.Id)
            .Single();
        return cleaner;
    }

    public async Task<string> GetProfilePicturePath()
    {
        var profilePicture = await getCleanerProfileHash();
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

    public async Task UpdateCleanerBio(string bioText)
    {
        var cleaner = await GetCurrentCleaner();

        var update = await _client
                       .From<CleanerModel>()
                       .Where(x => x.AuthId == cleaner.AuthId)
                       .Set(x => x.Bio, bioText)
                       .Update();
    }

    public async Task UploadProfilePicture(int retries = 0)
    {
        string photoDevicePath = await pickAPhotoFromFileSystem();
        string fileNameForSupabase = await getCleanerProfileHash();
        try
        { await storeProfilePictureInBucket(photoDevicePath, fileNameForSupabase);}
        catch (BadRequestException e)
        {
            if (e.ErrorResponse.Error == "Duplicate")
                await replaceProfilePictureInBucket(photoDevicePath, fileNameForSupabase);
        }
    }

    private async Task replaceProfilePictureInBucket(string photoToBeUploadedPath, string fileNameForSupabase)
    {
        await _client.Storage
            .From("profile-pictures")
            .Remove(new List<string> { fileNameForSupabase });
        await storeProfilePictureInBucket(photoToBeUploadedPath, fileNameForSupabase);
    }

    private async Task storeProfilePictureInBucket(string photoToBeUploadedPath, string fileNameForSupabase)
    {
        await _client.Storage
                      .From("profile-pictures")
                      .Upload(photoToBeUploadedPath, fileNameForSupabase);
    }

    private async Task<string> getCleanerProfileHash()
    {
        var cleaner = await GetCurrentCleaner();
        string supabaseUrl = "profile_picture_" + cleaner.AuthId[..5];
        return supabaseUrl;
    }

    private async Task<string> pickAPhotoFromFileSystem()
    {
        var res = await _platformService.PickFile();
        var photoUrl = res.FullPath;
        return photoUrl;
    }
}
