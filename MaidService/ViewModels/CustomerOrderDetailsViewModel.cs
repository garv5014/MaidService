using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.Views;

namespace MaidService.ViewModels;
[QueryProperty(nameof(Contract), nameof(Contract))]
public partial class CustomerOrderDetailsViewModel : ObservableObject, IQueryAttributable
{
    private ICustomerService _customer;
    private INavService _navService;

    public CustomerOrderDetailsViewModel(ICustomerService customer, INavService navService)
    {
        _customer = customer;
        _navService = navService;
    }
    [ObservableProperty]
    private CleaningContract contract;

    [ObservableProperty]
    private string cleanerName = null;

    private string allCleanersFirstNames(CleaningContract contract)
    {
        var allCleaners = contract?.AvailableCleaners;
        if (allCleaners?.Count > 0)
        {
            return allCleaners.First().Cleaner.FirstName;
        }
        return "No Cleaners Yet";
    }

    [RelayCommand]
    public async Task GoBack()
    {
        await _navService.NavigateTo($"///{nameof(CustomerProfile)}");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Contract = (CleaningContract)query[nameof(Contract)];
        CleanerName = allCleanersFirstNames(Contract);
    }
}