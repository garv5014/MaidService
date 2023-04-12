using AutoMapper;
using Maid.Library.Interfaces;
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

    public async Task AddCleaner(
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

    public async Task<Cleaner> GetCurrentCleaner()
    {
        var user = _auth.GetCurrentUser();

        var cleaner = await _client
            .From<CleanerModel>()
            .Filter("auth_id", Operator.Equals, user.Id)
            .Single();
        return cleaner.AuthId != null
            ? _mapper.Map<Cleaner>(cleaner)
            : null;
    }

    public async Task<string> GetProfilePicturePath()
    {
        var profilePicture = await GetCleanerProfileHash();
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
        string photoUrl = await PickAPhoto();
        string supabaseUrl = await GetCleanerProfileHash();
        try
        {
            await _client.Storage
              .From("profile-pictures")
              .Upload(photoUrl, supabaseUrl);
        }
        catch (BadRequestException e)
        {
            if (e.ErrorResponse.Error == "Duplicate")
            {
                var res = await _client.Storage
                    .From("profile-pictures")
                    .Remove(new List<string> { supabaseUrl });
                await _client.Storage
                    .From("profile-pictures")
                    .Upload(photoUrl, supabaseUrl);
            }
        }
    }

    private async Task<string> GetCleanerProfileHash()
    {
        var cleaner = await GetCurrentCleaner();
        string supabaseUrl = "profile_picture" + cleaner.AuthId[..5];
        return supabaseUrl;
    }

    private async Task<string> PickAPhoto()
    {
        var res = await _platformService.PickFile();
        var photoUrl = res.FullPath;
        return photoUrl;
    }
}
