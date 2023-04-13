using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.Views;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;

namespace MaidService.ViewModels;

public partial class CustomerScheduleViewModel : ObservableObject
{
    private ICustomerService _customerService;
    private readonly ICleanerService _cleanerService;
    private readonly IAuthService _auth;
    private readonly INavService _nav;
    [ObservableProperty]
    private bool isCleaner;

    [ObservableProperty]
    private string selectedCleaning;

    public CustomerScheduleViewModel(ICustomerService customerService, ICleanerService cleanerService, IAuthService auth, INavService nav)
    {
        _customerService = customerService;
        _cleanerService = cleanerService;
        _auth = auth;
        _nav = nav;
    }

    [ObservableProperty]
    private ObservableCollection<SchedulerAppointment> appointments;

    [RelayCommand]
    public async Task Appear()
    {
        Appointments = new();
        var role = await _auth.GetUserRole();
        if (role == "Customer")
        {
            await CustomerSetup();
        }
        else if (role == "Cleaner")
        {
            await CleanerSetup();
        }
    }

    [RelayCommand]
    public async Task NavigateToAllAvailableAppointments()
    {
        await _nav.NavigateTo($"////{nameof(AvailableCleanerAppointments)}");
    }

    private async Task CleanerSetup()
    {
        // get all cleaner appointments
        IsCleaner = true;
    }

    private async Task CustomerSetup()
    {
        IsCleaner = false;
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
