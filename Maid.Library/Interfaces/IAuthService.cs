using Supabase.Gotrue;

namespace Maid.Library.Interfaces;

public interface IAuthService
{
    public User GetUser(string email, string password);
    public Task<Session> SignInUser(string email, string password);
    public Task<Session> SignUpUser(string email, string password);
    public Task SignOutUser();
    public Task<User> UpdateEmail(string email);
    public Task<User> UpdatePassword(string password);
}
