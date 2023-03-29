using Maid.Library.Interfaces;
using Supabase;
using Supabase.Gotrue;

namespace MaidService.Services;

public class AuthenicationService : IAuthService
{
    private readonly Supabase.Client _client;

    public AuthenicationService(Supabase.Client client)
    {
        _client = client;
    }
    public User GetUser(string email, string password)
    {
        var user = _client.Auth.CurrentUser;
        return user;
    }

    public async Task<Session> SignInUser(string email, string password)
    {
        var res = await _client.Auth.SignIn(email, password);
        return res;
    }

    public async Task SignOutUser()
    {
        if (_client.Auth.CurrentUser != null)
            await _client.Auth.SignOut();
    }

    public Task<Session> SignUpUser(string email, string password)
    {
       var res = _client.Auth.SignUp(email, password);
       return res;
    }

    public async Task<User> UpdateEmail(string newEmail)
    {
        var attrs = new UserAttributes 
        { Email = newEmail };
        var response = await _client.Auth.Update(attrs);
        return response;
    }

    public async Task<User> UpdatePassword(string newPassword)
    {
        var attrs = new UserAttributes
        { Password = newPassword };
        var response = await _client.Auth.Update(attrs);
        return response;
    }
}
