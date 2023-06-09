﻿using Postgrest;
using Supabase.Gotrue;

namespace Maid.Library.Interfaces;

public interface IAuthService
{
    public Task<Session> SignInUser(string email, string password);
    public Task<Session> SignUpUser(string email, string password);
    public Task SignOutUser();
    public Task<User> UpdateEmail(string email);
    public Task<User> UpdatePassword(string password);
    public Task<string> GetUserRole();
    public User GetCurrentUser();

}
