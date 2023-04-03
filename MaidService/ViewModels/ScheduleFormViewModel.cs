using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using System.Collections.ObjectModel;

namespace MaidService.ViewModels;

public partial class ScheduleFormViewModel : ObservableObject
{
    private ICustomerService service;

    public ScheduleFormViewModel(ICustomerService service)
    {
        this.service = service;
    }

    [ObservableProperty]
    private ObservableCollection<CleaningType> cleaningTypes;

    [RelayCommand]
    public async Task Appear()
    {
        CleaningTypes = new();

        var allTypes = await service.GetCleaningTypes();
        foreach (var type in allTypes)
        {
            CleaningTypes.Add(new CleaningType
            {
                Id = type.Id,
                Type = type.Type,
                Description = type.Description,
            });
        }
    }

    [RelayCommand]
    public async Task AddJob()
    {

    }
}
