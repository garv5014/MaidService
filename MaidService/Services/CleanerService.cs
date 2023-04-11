using AutoMapper;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
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
}
