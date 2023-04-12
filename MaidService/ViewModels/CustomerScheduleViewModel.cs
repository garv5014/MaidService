using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;

namespace MaidService.ViewModels;

public partial class CustomerScheduleViewModel : ObservableObject
{
    private ICustomerService _customerService;
    private readonly ICleanerService _cleanerService;
    private readonly IAuthService _auth;

    [ObservableProperty]
    private string selectedCleaning;

    public CustomerScheduleViewModel(ICustomerService customerService, ICleanerService cleanerService, IAuthService auth)
    {
        _customerService = customerService;
        _cleanerService = cleanerService;
        _auth = auth;
    }

    [ObservableProperty]
    private ObservableCollection<SchedulerAppointment> appointments;

    [RelayCommand]
    public async Task Appear()
    {
        Appointments = new();
        var roles = await _auth.GetUserRoles();
        if (roles.Contains("Customer"))
        {
            await CustomerSetup();
        }
        else if (roles.Contains("Cleaner"))
        {
            await CleanerSetup();
        }
    }

    private async Task CleanerSetup()
    {
        var currentCleaner = await _cleanerService.GetCurrentCleaner();
    }

    private async Task CustomerSetup()
    {
        var contracts = await _customerService.GetAllAppointments();
        foreach (var schedule in contracts)
        {
            Appointments.Add(new SchedulerAppointment
            {
                Id = schedule.Id,
                StartTime = schedule.ScheduleDate,
                EndTime = schedule.ScheduleDate + schedule.RequestedHours,
                IsAllDay = false,
                Subject = schedule?.CleaningType.Type,
                Background = await _customerService.IsScheduled(schedule.Id) ?
                                                Brush.Red
                                                : Brush.Green,
            });
        }
    }
}
