using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Views;

namespace MaidService.ViewModels;

public partial class LoginPageViewModel : ObservableObject
{
    private readonly INavService _nav;
    private readonly IAuthService _auth;
    [ObservableProperty]
    private string userEmail;
    [ObservableProperty]
    private string password;
    [ObservableProperty]
    private string loginResponse;

    public LoginPageViewModel(INavService nav, IAuthService auth)
    {
        _nav = nav;
        _auth = auth;
    }

    [RelayCommand]
    public async Task AttemptLogin()
    {
        var session = await _auth.SignInUser(UserEmail, Password);
        if (session != null)
        {
            var roles = await _auth.GetUserRoles();
            if (roles.Contains("Cleaner"))
            {
                //await _nav.NavigateTo($"///{nameof(CleanerProfile)}");
            }
            else if (roles.Contains("Customer"))
            {
                await _nav.NavigateTo($"///{nameof(CustomerProfile)}");
            }
        }
        else
        {
            LoginResponse = "Failed To Login";
        }
    }

    [RelayCommand]
    public async Task NavigateToSignUP()
    {
        await _nav.NavigateTo($"///{nameof(SignUpPage)}");
    }
}
