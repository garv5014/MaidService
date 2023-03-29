using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.ComponentsViewModels;

namespace MaidService.ViewModels;

public partial class CustomerProfileViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;
    [ObservableProperty]
    private IEnumerable<AppointmentCardViewModel> appointments;

    [ObservableProperty]
    private string appointmentsHeader;

    public CustomerProfileViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
        AppointmentsHeader = "No Upcoming Appointments";
    }

    [RelayCommand]
    public async Task Appear()
    {
        var res = await _customerService.GetUpcomingAppointments(1);
        Appointments = new List<AppointmentCardViewModel>();
        if (res != null)
        {
            Appointments = res.Select(a => new AppointmentCardViewModel(a));
        }

        if (Appointments.Count() > 0 )
        {
            AppointmentsHeader = "Upcoming Appointments";
        }
    }
}
