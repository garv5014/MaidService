using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.Views;

namespace MaidService.ViewModels;

//[QueryProperty(nameof(Contract), nameof(Contract))]
public partial class CleanerAddAppointmentViewModel : ObservableObject, IQueryAttributable
{
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Contract = (CleaningContract)query[nameof(Contract)];
        AvailableTimes = await _cleanerService.GetCleanerAvailabilityForASpecificContract(Contract);
        if (AvailableTimes.Count() == 0)
        {
            await Shell.Current.DisplayAlert("No Available Times", "There are no available times for this contract", "OK");
            await Shell.Current.GoToAsync($"///{nameof(AvailableCleanerAppointments)}");
        }
    }
    private ICleanerService _cleanerService;
    [ObservableProperty]
    private Schedule selectedSlot;

    public CleanerAddAppointmentViewModel(ICleanerService cleanerService)
    {
        _cleanerService = cleanerService;
    }

    public CleaningContract Contract { get => contract ; 
        set 
        {
            SetProperty(ref contract, value);
        } 
    }

    private CleaningContract contract;

    [ObservableProperty]
    private IEnumerable<Schedule> availableTimes;

    [RelayCommand]
    private async Task AddAssignedSchedule()
    {
        if (SelectedSlot == null)
        {
            await Shell.Current.DisplayAlert("No Schedule Selected", "Please select a start time", "OK");
            return;
        }
        await _cleanerService.UpdateCleanerAssignments(Contract.Id, SelectedSlot);
    }

}
