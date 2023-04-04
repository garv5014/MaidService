using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using System.Collections.ObjectModel;

namespace MaidService.ViewModels;

public partial class ScheduleFormViewModel : ObservableObject
{
    private ICustomerService _customerService;
    private IPlatformService _platform;

    [ObservableProperty]
    private int selectedIndex = 0;

    public ScheduleFormViewModel(ICustomerService service, IPlatformService platform)
    {
        _customerService = service;
        _platform = platform;
    }

    [ObservableProperty]
    private CleaningContract contract = new() { ScheduleDate = DateTime.Now };
    
    [ObservableProperty]
    private ObservableCollection<CleaningType> cleaningTypes;

    [ObservableProperty]
    private int requestedHours;

    [ObservableProperty]
    private DateTime minDate = DateTime.Now + TimeSpan.FromDays(3);

    [RelayCommand]
    public async Task Appear()
    {
        CleaningTypes = new() { new CleaningType {Type = "Loading..." } } ;
        var tempTypes = new ObservableCollection<CleaningType>();
        var allTypes = await _customerService.GetCleaningTypes();
        foreach (var type in allTypes)
        {
            tempTypes.Add(new CleaningType
            {
                Id = type.Id,
                Type = type.Type,
                Description = type.Description,
            });
        }
        CleaningTypes = tempTypes;
    }

    [RelayCommand]
    public async Task AddJob()
    {
        // make sure the required fields are filled out
        if (!IsValidContract())
        {
            // if not, show a message to the user
        }
        else 
        {
            // if all fields are good make add the contract to the database
            // and show a success message to the user
            Contract.RequestedHours = TimeSpan.FromHours(RequestedHours);
            Contract.CleaningType = new CleaningType { Type = CleaningTypes[SelectedIndex].Type, Id = CleaningTypes[SelectedIndex].Id };
            await _customerService.CreateNewContract(Contract);
            _platform.DisplayAlert("All Done", "Your appoinment was scheduled", "Ok");
        
        }

    }

    private bool IsValidContract()
    {
        var res = !string.IsNullOrEmpty(Contract.Location.Address)
            && !string.IsNullOrEmpty(Contract.Location.City)
            && !string.IsNullOrEmpty(Contract.Location.State)
            && !string.IsNullOrEmpty(Contract.Location.ZipCode)
            && Contract.EstSqft > 0
            && RequestedHours >= 1
            && Contract.ScheduleDate > DateTime.Now;
        return res;
    }
}
