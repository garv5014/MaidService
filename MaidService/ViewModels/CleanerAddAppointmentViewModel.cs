using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.Views;

namespace MaidService.ViewModels;

//[QueryProperty(nameof(Contract), nameof(Contract))]
public partial class CleanerAddAppointmentViewModel : ObservableObject, IQueryAttributable
{

    private ICleanerService _cleanerService;
    private readonly INavService _navService;

    public CleanerAddAppointmentViewModel(ICleanerService cleanerService, INavService navService)
    {
        _cleanerService = cleanerService;
        _navService = navService;
    }

    [ObservableProperty]
    private Schedule selectedSlot;

    [ObservableProperty]
    private IEnumerable<Schedule> availableTimes;

    [ObservableProperty]
    private bool isLoading = true;

    public CleaningContract Contract { get => contract ; 
        set 
        {
            SetProperty(ref contract, value);
        } 
    }

    private CleaningContract contract;

    [RelayCommand]
    private async Task AddAssignedSchedule()
    {
        if (SelectedSlot == null)
        {
            await Shell.Current.DisplayAlert("No Schedule Selected", "Please select a start time", "OK");
            return;
        }
        await _cleanerService.UpdateCleanerAssignments(Contract, SelectedSlot);
        NavigateBackToSchedule();
    }

    [RelayCommand]
    public async Task NavigateToCleanerDetails()
    {
        await _navService.NavigateToWithParameters(
            $"///{nameof(CleanerOrderDetails)}",
            new Dictionary<string, object> { { "Contract", Contract } }
        );
    }

    private async Task NavigateBackToSchedule()
    {
        await _navService.NavigateTo($"///CleanerSchedule");
    }
    
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        IsLoading = true;

        Contract = (CleaningContract)query[nameof(Contract)];
        AvailableTimes = await _cleanerService.GetCleanerAvailabilityForASpecificContract(Contract);
        if (AvailableTimes.Count() == 0)
        {
            await Shell.Current.DisplayAlert("No Available Times", "There are no available times for this contract", "OK");
            await Shell.Current.GoToAsync($"///{nameof(AvailableCleanerAppointments)}");
        }

        IsLoading = false;
    }
}
