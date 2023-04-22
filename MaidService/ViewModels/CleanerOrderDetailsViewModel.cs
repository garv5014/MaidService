using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.Views;

namespace MaidService.ViewModels;

public partial class CleanerOrderDetailsViewModel : ObservableObject, IQueryAttributable
{
    private ICleanerService _cleanerService;
    private ICustomerService _customer;
    private INavService _navService;

    public CleanerOrderDetailsViewModel(ICleanerService cleaner, ICustomerService customer, INavService navService)
    {
        _cleanerService = cleaner;
        _customer = customer;
        _navService = navService;
    }

    [ObservableProperty]
    private CleaningContract contract;

    [ObservableProperty]
    private string cleanerNames = null;

    [ObservableProperty]
    private string customerPhoneNumber;

    [ObservableProperty]
    private bool isCleanerAssignedToContract = false;

    [ObservableProperty]
    private bool isAddButtonVisible = true;

    private string allCleanersFirstNames(CleaningContract result)
    {
        var allCleaners = result?.AvailableCleaners;
        if (allCleaners?.Count > 0)
        {
            return allCleaners.First().Cleaner.FirstName;
        }
        return "No Cleaners Yet";
    }

    [RelayCommand]
    public async Task GoBack()
    {
        if (IsCleanerAssignedToContract)
            await _navService.NavigateTo($"///{nameof(CleanerProfile)}");
        else
            await _navService.NavigateTo($"///{nameof(AvailableCleanerAppointments)}");
    }

    [RelayCommand]
    public async Task NavigateToAddAppoinmentPage()
    {
        await _navService.NavigateToWithParameters($"///{nameof(CleanerAddAppointment)}",
            new Dictionary<string, object>
            {
                { "Contract", Contract }
            }
            );
    }

    [RelayCommand]
    public async Task CancelAppointment()
    {
        await _cleanerService.RemoveCleanerFromAppointment(Contract.Id);
        await _navService.NavigateTo($"///{nameof(CleanerProfile)}");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Contract = (CleaningContract)query[nameof(Contract)];
        CleanerNames = allCleanersFirstNames(Contract);
        CustomerPhoneNumber = Contract.Customer.PhoneNumber;

        if (CleanerNames == "No Cleaners Yet")
        {
            IsCleanerAssignedToContract = false;
            IsAddButtonVisible = true;
        }
        else
        {
            IsCleanerAssignedToContract = true;
            IsAddButtonVisible = false;
        }
    }
}
