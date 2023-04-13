using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using Supabase;
using Supabase.Gotrue;
using static Postgrest.Constants;

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

    public async Task<string> GetUserRole()
    {
        if (await IsCustomer())
        {
            return "Customer";
        }

        if (await IsCLeaner())
        {
            return "Cleaner";
        }
        return "";
    }

    private async Task<bool> IsCLeaner()
    {
        var cleaner = await _client
            .From<CleanerModel>()
            .Filter("auth_id", Operator.Equals, _client.Auth.CurrentUser.Id)
            .Single();
        return cleaner?.AuthId != null;
    }

    private async Task<bool> IsCustomer()
    {
        var customer = await _client
                        .From<CustomerModel>()
                        .Filter("auth_id", Operator.Equals, _client.Auth.CurrentUser.Id)
                        .Single();
        return customer?.AuthId != null;
    }

    public async Task<Session> SignInUser(string email, string password)
    {
        email = HandelEmptyInput(email);
        password = HandelEmptyInput(password);
        Session res = null;
        try
        {
            res = await _client.Auth.SignIn(email, password);
        }
        catch (Exception e)
        {

            return res;
        }
        return res;
    }

    private static string HandelEmptyInput(string input)
    {
        return string.IsNullOrEmpty(input)
                    ? ""
                    : input.Trim();
    }

    public async Task SignOutUser()
    {
        if (_client.Auth.CurrentUser != null)
            await _client.Auth.SignOut();
    }

    public async Task<Session> SignUpUser(string email, string password)
    {
        email = email.Trim();
        password = password.Trim();
        Session session = null;
        try
        {
            session = await _client.Auth.SignUp(email, password);
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
