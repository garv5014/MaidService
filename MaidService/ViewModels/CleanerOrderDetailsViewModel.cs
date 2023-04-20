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
    private bool isCleanerAssignedToContract = false;

    [ObservableProperty]
    private bool isAddButtonVisible = true;

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

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Contract = (CleaningContract)query[nameof(Contract)];
        CleanerNames = allCleanersFirstNames(Contract);

        if (CleanerNames != null)
        {
            IsCleanerAssignedToContract = true;
            IsAddButtonVisible = false;
        }
        else
        {
            IsCleanerAssignedToContract = false;
            IsAddButtonVisible = true;
        }
    }
}
