using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Views;

namespace MaidService.ViewModels;

public partial class LoginPageViewModel : ObservableObject
{
    private readonly INav _nav;
    private readonly IAuthService _auth;

    [ObservableProperty]
    private string userEmail;
    [ObservableProperty]
    private string password;

    public LoginPageViewModel(INav nav, IAuthService auth)
    {
        _nav = nav;
        _auth = auth;
    }

    [RelayCommand]
    public async Task AttemptLogin()
    {
        var session = await _auth.SignInUser(UserEmail,Password);
        
    }

    [RelayCommand]
    public async Task NavigateToSignUP()
    {
        await _nav.NavigateTo($"///{nameof(SignUpPage)}");
    }
}
