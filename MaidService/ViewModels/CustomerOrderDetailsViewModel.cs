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

    [ObservableProperty]
    private bool isLoading = true;

    [RelayCommand]
    public async Task GoBack()
    {
        await _navService.NavigateTo($"///{nameof(CustomerProfile)}");
    }

    private string allCleanersFirstNames(CleaningContract contract)
    {
        var allCleaners = contract?.AvailableCleaners;
        if (allCleaners?.Count > 0)
        {
            return allCleaners.First().Cleaner.FirstName;
        }
        return "No Cleaners Yet";
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        IsLoading = true;
        Contract = new();
        Contract = (CleaningContract)query[nameof(Contract)];
        CleanerName = allCleanersFirstNames(Contract);

        IsLoading = false;
    }
}