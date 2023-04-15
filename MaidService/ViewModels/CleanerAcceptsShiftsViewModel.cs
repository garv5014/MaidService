using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;

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

    public CleanerAcceptsShiftsViewModel(ICleanerService cleanerService)
    {
        _cleanerService = cleanerService;
    }

    [RelayCommand]
    private async Task Appear()
    {
        ScheduleDate = DateTime.Now;
        await GetSchedulesForADate();
    }

    [RelayCommand]
    private async Task GetSchedulesForADate()
    { 
        Schedules = await _cleanerService.GetSchedulesForADate(ScheduleDate);
    }
}
