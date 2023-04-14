using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.ComponentsViewModels;
using System.Collections.ObjectModel;

namespace MaidService.ViewModels;

public partial class AvailableCleanerAppointmentsViewModel : ObservableObject
{
    private readonly INavService _nav;
    private readonly ICleanerService _cleanerService;

    [ObservableProperty]
    private string appointmentsHeader = "No Available Appointments";

    [ObservableProperty]
    private IEnumerable<CleanerAppointmentCardViewModel> appointments;

    public AvailableCleanerAppointmentsViewModel(INavService nav, ICleanerService cleanerService)
    {
        _nav = nav;
        _cleanerService = cleanerService;
    }

    [RelayCommand]
    public async Task Appear()
    {
       var appointments = await _cleanerService.GetAllAvailableAppointments();

        if (appointments != null)
        {
            Appointments = appointments.Select(a => new CleanerAppointmentCardViewModel(a, _nav));
        }

        if (Appointments?.Count() > 0)
        {
            AppointmentsHeader = "Available Appointments";
        }
    }
}
