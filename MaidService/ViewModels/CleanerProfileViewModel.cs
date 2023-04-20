using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.Views;

namespace MaidService.ViewModels;

public partial class CleanerProfileViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;
    private readonly INavService _nav;
    private readonly ICleanerService _cleanerService;
    private readonly ISupabaseStorage _storage;

    public CleanerProfileViewModel(
        ICustomerService customerService, 
        INavService nav, 
        ICleanerService cleanerService, 
        ISupabaseStorage storage)
    {
        _customerService = customerService;
        _nav = nav;
        _cleanerService = cleanerService;
        _storage = storage;
        AppointmentsHeader = "No Upcoming Appointments";
    }

    [ObservableProperty]
    private Cleaner currentCleaner = new();

    [ObservableProperty]
    private CleaningContract cleanerContract;

    [ObservableProperty]
    private IEnumerable<CleaningContractWithStartTime> appointments;

    [ObservableProperty]
    private string appointmentsHeader;

    [ObservableProperty]
    private bool isEditing = false;

    [ObservableProperty]
    private bool isNotEditing = true;

    [ObservableProperty]
    private string bioText;
    
    private void ToggleEditing()
    {
        IsEditing = !IsEditing;
        IsNotEditing = !IsNotEditing;
    }

    [RelayCommand]
    public async Task Appear()
    {
        CurrentCleaner = await _cleanerService.GetCurrentCleaner();
        BioText = CurrentCleaner.Bio;

        var res = await _cleanerService.GetUpcomingAppointments();

        if (res != null)
        {
            Appointments = res;
        }

        if (Appointments.Count() > 0)
        {
            AppointmentsHeader = "Upcoming Appointments";
        }
    }

    [RelayCommand]
    public void EditBio()
    {
        ToggleEditing();
    }

    [RelayCommand]
    public async Task UpdateBio()
    {
        ToggleEditing();
        if (!string.IsNullOrEmpty(BioText))
        { 
            await _cleanerService.UpdateCleanerBio(BioText);
            await Appear();
        }
    }

    [RelayCommand]
    public async Task TapCard(CleaningContractWithStartTime contract)
    {
        var result = await _cleanerService.GetCleaningContractDetails(contract.Id);

        await _nav.NavigateToWithParameters($"///{nameof(CleanerOrderDetails)}",
            new Dictionary<string, object>
            {
                { "Contract", result }
            }
            );
    }
}
