using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.ComponentsViewModels;

namespace MaidService.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    public AppointmentCardViewModel TestAppointment { get; set; } = new AppointmentCardViewModel { Address = "This is just a drill" };

    [ObservableProperty]
    private string text;

    private readonly ICustomerService _customerService;

    public MainPageViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [RelayCommand]
    public async Task GetCustomers()
    {
        var res = await _customerService.GetUpcomingAppointments();
        if (res == null)
        {

        }
        else
        {
            Text = res[0].Location.Address;
        }
    }

}
