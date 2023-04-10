﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Views;

namespace MaidService.ViewModels;

public partial class LoginPageViewModel : ObservableObject
{
    private readonly INavService _nav;
    private readonly IAuthService _auth;
    private readonly IServiceProvider _services;
    [ObservableProperty]
    private string userEmail;
    [ObservableProperty]
    private string password;
    [ObservableProperty]
    private string loginResponse;

    public LoginPageViewModel(INavService nav, IAuthService auth, IServiceProvider services)
    {
        _nav = nav;
        _auth = auth;
        _services = services;
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
                App.Current.MainPage = new AppShell();
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
        //await _nav.NavigateTo($"///{nameof(SignUpPage)}");
        App.Current.MainPage = new SignUpPage(_services.GetRequiredService<SignUpPageViewModel>());
    }
}
