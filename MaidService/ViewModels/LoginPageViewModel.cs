using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.Interfaces;
using MaidService.Views;

namespace MaidService.ViewModels;

public partial class LoginPageViewModel : ObservableObject
{
    private readonly INavService _nav;
    private readonly IAuthService _auth;
    private readonly IServiceProvider _services;
    private readonly IApiService _api;

    public LoginPageViewModel(INavService nav,
        IAuthService auth,
        IServiceProvider services,
        IApiService api)
    {
        _nav = nav;
        _auth = auth;
        _services = services;
        _api = api;
    }

    [ObservableProperty]
    private string welcomeMessage;

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
