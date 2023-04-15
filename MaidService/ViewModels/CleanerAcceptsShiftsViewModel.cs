using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using System.Collections.ObjectModel;

namespace MaidService.ViewModels;

public partial class CleanerAcceptsShiftsViewModel : ObservableObject
{
    private readonly ICleanerService _cleanerService;
    [ObservableProperty]
    private DateTime minDate = DateTime.Now;

    [ObservableProperty]
    private DateTime scheduleDate;

    [ObservableProperty]
    private IEnumerable<Schedule> schedules;

    [ObservableProperty]
    private List<object> selectedSlots;

    [ObservableProperty]
    private bool existsAvailableAppoinments;

    public CleanerAcceptsShiftsViewModel(ICleanerService cleanerService)
    {
        _cleanerService = cleanerService;
    }


    public CleanerAcceptsShiftsViewModel()
    {
        ScheduleDate = DateTime.Now;
    }
    [RelayCommand]
    private async Task Appear()
    {
        SelectedSlots = new List<object>();
        await GetSchedulesForADate();
        ExistsAvailableAppoinments = Schedules.Count() != 0;
    }

    [RelayCommand]
    private async Task GetSchedulesForADate()
    { 
        Schedules = await _cleanerService.GetAvailableSchedulesForADate(ScheduleDate);
    }
    [RelayCommand]
    private async Task AddSelectedToSchedule()
    {
        await _cleanerService.UpdateCleanerAvailablility(SelectedSlots);
        await Appear();
    }
}
