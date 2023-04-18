using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.Views;

namespace MaidService.ViewModels;
[QueryProperty(nameof(Contract), nameof(Contract))]
public partial class CleanerOrderDetailsViewModel : ObservableObject
{
    private ICustomerService _customer;
    private INavService _navService;

    public CleanerOrderDetailsViewModel(ICustomerService customer, INavService navService)
    {
        _customer = customer;
        _navService = navService;
    }

    [ObservableProperty]
    private CleaningContract contract;

    [ObservableProperty]
    private string cleanerNames = null;

    [RelayCommand]
    public void NavigatedTo()
    {
        CleanerNames = allCleanersFirstNames(Contract);
    }

    private string allCleanersFirstNames(CleaningContract result)
    {
        var allCleaners = result?.AvailableCleaners;
        List<string> allCleanersNames = new();
        if (allCleaners?.Count > 0)
        {
            foreach (var cleaner in allCleaners)
            {
                allCleanersNames.Add(cleaner.Cleaner.FirstName);
            }
            var res = string.Join(", ", allCleanersNames);
            return res;
        }
        return "No Cleaners Yet";
    }

    [RelayCommand]
    public async Task GoBack()
    {
        await _navService.NavigateTo($"///{nameof(AvailableCleanerAppointments)}");
    }

    [RelayCommand]
    public async Task NavigateToAddAppoinmentPage()
    {
        await _navService.NavigateTo($"///{nameof(CleanerAddAppointment)}?ContractId={Contract.Id}");
    }
}
