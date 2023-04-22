using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.ComponentsViewModels;

namespace MaidService.ViewModels;

public partial class AvailableCleanerAppointmentsViewModel : ObservableObject
{
    private readonly INavService _nav;
    private readonly ICleanerService _cleanerService;

    public AvailableCleanerAppointmentsViewModel(INavService nav, ICleanerService cleanerService)
    {
        _nav = nav;
        _cleanerService = cleanerService;
    }

    [ObservableProperty]
    private string appointmentsHeader = "No Available Appointments";

    [ObservableProperty]
    private IEnumerable<CleanerAppointmentCardViewModel> appointments;

    [ObservableProperty]
    private bool isLoading = true;

    [RelayCommand]
    public async Task Appear()
    {
        IsLoading = true;

        var appointments = await _cleanerService.GetAllAvailableAppointments();

        if (appointments != null)
        {
            Appointments = appointments.Select(a => new CleanerAppointmentCardViewModel(a, _nav));
        }

        if (Appointments?.Count() > 0)
        {
            AppointmentsHeader = "Available Appointments";
        }

        IsLoading = false;
    }

    [RelayCommand]
    public async Task BackToSchedule()
    {
        await _nav.NavigateTo($"///CleanerSchedule");
    }
}
