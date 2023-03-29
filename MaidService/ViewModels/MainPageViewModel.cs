using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.ComponentsViewModels;
using Supabase;

namespace MaidService.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string text;

    private readonly ICustomerService _customerService;
    private readonly Client client;

    public MainPageViewModel(ICustomerService customerService, Supabase.Client client)
    {
        _customerService = customerService;
        this.client = client;
    }

    [RelayCommand]
    public async Task GetCustomers()
    {
        var res = await _customerService.GetUpcomingAppointments(1);
        var signIn = await client.Auth.SignIn("fake@gmail.com", "fake");
        if (res == null)
        {

        }
        else
        {
            Text = res.First().Location.Address;
        }
    }

}
