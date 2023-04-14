using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.Views;
using System.Reactive.Linq;

namespace MaidService.ViewModels;
[QueryProperty(nameof(ContractId), nameof(ContractId))]
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
    private string cleanerNames = null;

    [ObservableProperty]
    private string price = null;

    [ObservableProperty]
    private string scheduledTime = null;

    [ObservableProperty]
    private string timeDuration = null;

    [ObservableProperty]
    private string typeOfCleaning = null;

    [ObservableProperty]
    private string location = null;

    [ObservableProperty]
    private string notes = null;

    [ObservableProperty]
    private int contractId;

    [RelayCommand]
    public async Task NavigatedTo()
    {
        var result = await _customer.GetCleaningDetailsById(ContractId);

        if (result?.Id > 0)
        {
            Price = $"{result.Cost}";
            ScheduledTime = result.ScheduleDate.ToString("M/d/yyyy H:mm tt");
            TimeDuration = $"{result.RequestedHours.Hours}:{result.RequestedHours.Minutes.ToString("D2")}";
            TypeOfCleaning = result.CleaningType.Type;
            Location = $"{result.Location.Address}, {result.Location.City}, {result.Location.State}";
            Notes = result.Notes;
            CleanerNames = allCleanersFirstNames(result);
        }
    }

    private string allCleanersFirstNames(CleaningContract result)
    {
        var allCleaners = result.Cleaners;
        List<string> allCleanersNames = new();
        if (allCleaners.Count > 0)
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
        await _navService.NavigateTo($"///{nameof(CustomerOrderDetails)}?ContractId={ContractId}");
    }
}
