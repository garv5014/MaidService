using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Views;
using System.Collections.ObjectModel;

namespace MaidService.ViewModels;

public partial class SignUpPageViewModel : ObservableObject
{
    private readonly INavService _nav;
    private readonly IAuthService _auth;
    private readonly ICustomerService _customerService;

    [ObservableProperty]
    private string userEmail;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string phoneNumber;

    [ObservableProperty]
    private string firstName;

    [ObservableProperty]
    private string lastName;

    [ObservableProperty]
    private string errorMessage;

    [ObservableProperty]
    private string selectedAccountType;

    public List<string> AccountTypes { get; set; } = new(); 
    public SignUpPageViewModel(INavService nav, IAuthService auth, ICustomerService customerService)
    {
        _nav = nav;
        _auth = auth;
        _customerService = customerService;
        AccountTypes.Add("Cleaner");
        AccountTypes.Add("Customer");
    }

    [RelayCommand]
    public async Task  SignUpUser()
    { 

        if (AreFieldsValid()) 
        {
            var session =  await _auth.SignUpUser(UserEmail, Password);

            if (SelectedAccountType == "Cleaner" && session != null)
            {
                //await _customerService.AddCleaner(FirstName, LastName, PhoneNumber, UserEmail);
                //await NavigateToCleanerTabs();
            }
            else if (SelectedAccountType == "Customer" && session != null)
            {
                await _customerService.AddCustomer(FirstName, LastName, PhoneNumber, UserEmail, session.User.Id);
            }
            
            await NavigateToLogin();
        }
        else 
        {
            ErrorMessage = "Please fill out all fields";
        }
        // if successful add them to the the respective table
        // then navigate to the correct nav bar.
    }

    [RelayCommand]
    public async Task  NavigateToLogin()
    { 
        await _nav.NavigateTo($"///{nameof(LoginPage)}");
    }

    internal bool AreFieldsValid()
    { 
        var res = !string.IsNullOrEmpty(UserEmail)
                && !string.IsNullOrEmpty(Password)
                && !string.IsNullOrEmpty(PhoneNumber)
                && !string.IsNullOrEmpty(FirstName)
                && !string.IsNullOrEmpty(LastName)
                && !string.IsNullOrEmpty(SelectedAccountType);
        return res; 
    }
}
