using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Views;

namespace MaidService.ViewModels;

public partial class LoginPageViewModel : ObservableObject
{
    private readonly INavService _nav;
    private readonly IAuthService _auth;
    private readonly IServiceProvider _services;

    public LoginPageViewModel(INavService nav, IAuthService auth, IServiceProvider services)
    {
        _nav = nav;
        _auth = auth;
        _services = services;
    }

    [ObservableProperty]
    private string userEmail;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string loginResponse;

    [ObservableProperty]
    private bool isLoading = true;

    [RelayCommand]
    public async Task AttemptLogin()
    {
        var session = await _auth.SignInUser(UserEmail, Password);
        if (session != null)
        {
            var role = await _auth.GetUserRole();
            if (role == "Cleaner")
            {
                await _nav.NavigateTo($"///{nameof(CleanerProfile)}");
            }
            else if (role == "Customer")
            {
                await _nav.NavigateTo($"///{nameof(CustomerProfile)}");
            }
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
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
