using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.ComponentsViewModels;
using MaidService.Library.DbModels;

namespace MaidService.ViewModels;

public partial class CustomerProfileViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;
    private readonly ISupabaseStorage _storage;
    private readonly INavService _nav;
    private readonly IPlatformService _platform;

    [ObservableProperty]
    private Customer currentCustomer = new();

    [ObservableProperty]
    private CleaningContract _customerContract;

    [ObservableProperty]
    private IEnumerable<CustomerAppointmentCardViewModel> appointments;

    [ObservableProperty]
    private string appointmentsHeader;

    [ObservableProperty]
    private bool isLoading = true;

    public CustomerProfileViewModel(ICustomerService customerService, ISupabaseStorage storage, INavService nav, IPlatformService platform)
    {
        _customerService = customerService;
        _storage = storage;
        _nav = nav;
        _platform = platform;
        AppointmentsHeader = "No Upcoming Appointments";
    }

    [RelayCommand]
    public async Task Appear()
    {
        IsLoading = true;

        CurrentCustomer = await _customerService.GetCurrentCustomer();
        var res = await _customerService.GetUpcomingAppointments(CurrentCustomer.Id);
        if (res != null)
        {
            Appointments = res.Select(a => new CustomerAppointmentCardViewModel(a, _nav, _storage));
        }

        if (Appointments.Count() > 0 )
        {
            AppointmentsHeader = "Upcoming Appointments";
        }
        IsLoading = false;
    }

    [RelayCommand]
    public async Task UploadPicture()
    {
        await _storage.UploadProfilePicture();
    }
}
