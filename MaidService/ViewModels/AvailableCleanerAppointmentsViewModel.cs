using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.ComponentsViewModels;
using System.Collections.ObjectModel;

namespace MaidService.ViewModels;

partial class AvailableCleanerAppointmentsViewModel : ObservableObject
{
    private readonly INavService _nav;
    private readonly ICleanerService _cleanerService;

    [ObservableProperty]
    private string appointmentsHeader;

    [ObservableProperty]
    private ObservableCollection<CleanerAppointmentCardViewModel> appointments;

    public AvailableCleanerAppointmentsViewModel(INavService nav, ICleanerService cleanerService)
    {
        _nav = nav;
        _cleanerService = cleanerService;
    }

    [RelayCommand]
    public async Task Appear()
    {
        var result = await _cleanerService.GetAllAvailableAppointments();
    }
}
