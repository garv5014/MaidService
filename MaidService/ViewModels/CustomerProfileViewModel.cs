using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.ComponentsViewModels;
using MaidService.Library.DbModels;

namespace MaidService.ViewModels;

public partial class CustomerProfileViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;
    private readonly INavService _nav;

    [ObservableProperty]
    private Customer currentCustomer = new();

    [ObservableProperty]
    private CleaningContract customerContract;

    [ObservableProperty]
    private IEnumerable<AppointmentCardViewModel> appointments;

    [ObservableProperty]
    private string appointmentsHeader;

    public CustomerProfileViewModel(ICustomerService customerService, INavService nav)
    {
        _customerService = customerService;
        _nav = nav;
        AppointmentsHeader = "No Upcoming Appointments";
    }

    [RelayCommand]
    public async Task Appear()
    {
        CurrentCustomer = await _customerService.GetCurrentCustomer();
        var res = await _customerService.GetUpcomingAppointments(CurrentCustomer.Id);
        if (res != null)
        {
            Appointments = res.Select(a => new AppointmentCardViewModel(a, _nav));
        }

        if (Appointments.Count() > 0 )
        {
            AppointmentsHeader = "Upcoming Appointments";
        }
    }

    [RelayCommand]
    public async Task UploadPicture()
    {
        await _customerService.UploadPhoto();
    }
}
