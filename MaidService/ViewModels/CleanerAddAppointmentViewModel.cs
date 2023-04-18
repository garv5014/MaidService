using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.Views;

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
    private CleaningContract contract;

    [ObservableProperty]
    private IEnumerable<Schedule> availableTimes;

    [ObservableProperty]
    private object selectedSlot;

    [RelayCommand]
    public async Task Appear()
    {
        AvailableTimes = await _cleanerService.GetCleanerAvailabilityForASpecificContract(Contract);
        if (AvailableTimes.Count() == 0)
        {
            await Shell.Current.DisplayAlert("No Available Times", "There are no available times for this contract", "OK");
            await Shell.Current.GoToAsync($"///{nameof(AvailableCleanerAppointments)}");
        }
    }

    [RelayCommand]
    private async Task AddAssignedSchedule()
    {
        await _cleanerService.UpdateCleanerAssignments(Contract.Id, SelectedSlot);
    }
}
