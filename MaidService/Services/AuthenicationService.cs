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

    public User GetCurrentUser()
    {
        return _client.Auth.CurrentUser;
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

    public async Task<Session> SignUpUser(string email, string password)
    {
        Session session = null;
        try
        {
            session =  await _client.Auth.SignUp(email, password);
        }
        catch (Exception e)
        {
            return session;
        }
        return session;
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
