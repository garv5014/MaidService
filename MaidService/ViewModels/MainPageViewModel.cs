using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.ComponentsViewModels;
using Supabase;

namespace MaidService.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    public AppointmentCardViewModel TestAppointment { get; set; } = new AppointmentCardViewModel { Address = "This is just a drill" };

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
        var res = await _customerService.GetUpcomingAppointments();
        var signIn = await client.Auth.SignIn("fake@gmail.com", "fake");
        if (res == null)
        {

        }
        else
        {
            Text = res[0].Location.Address;
        }
    }

}
