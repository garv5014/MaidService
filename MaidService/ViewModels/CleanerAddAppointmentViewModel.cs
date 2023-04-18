using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;

namespace MaidService.ViewModels;

[QueryProperty(nameof(Contract), nameof(Contract))]
public partial class CleanerAddAppointmentViewModel : ObservableObject
{
    private ICleanerService _cleanerService;

    public CleanerAddAppointmentViewModel(ICleanerService cleanerService)
    {
        _cleanerService = cleanerService;
    }
    [ObservableProperty]
    CleaningContract contract;

    [ObservableProperty]
    private IEnumerable<Schedule> availableTimes;

    [RelayCommand]
    private async Task Appear()
    {
        AvailableTimes = await _cleanerService.GetCleanerAvailabilityForAContract(Contract);
    }

    [RelayCommand]
    private async Task AddAssignedSchedule()
    {
        //await _cleanerService.UpdateCleanerAssignments(Contract.Id, SelectedSlot);
    }
}
