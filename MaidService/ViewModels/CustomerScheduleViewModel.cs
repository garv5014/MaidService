using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.Views;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

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
            DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            int daysTillCurrentDay = currentDay - DayOfWeek.Monday;
            DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay);
            await CleanerSetup(currentWeekStartDate);
        }
    }

    [RelayCommand]
    public async Task NavigateToAllAvailableAppointments()
    {
        await _nav.NavigateTo($"////{nameof(AvailableCleanerAppointments)}");
    }

    private async Task CleanerSetup(DateTime startDate)
    {
        IsCleaner = true;
        var appointments = await _cleanerService.GetAllScheduledAppointmentsForAWeek(startDate); 

        var convertedAppointments = new List<SchedulerAppointment>();
        foreach (var appointment in appointments)
        {
            var isScheduled = appointment.Id != 0;
            var schedAppointment = new SchedulerAppointment
            {
                Id = appointment.Id,
                StartTime = new DateTime(
                        year: appointment.ScheduleDate.Year,
                        month: appointment.ScheduleDate.Month,
                        day: appointment.ScheduleDate.Day,
                        hour: appointment.StartTime.Hours,
                        minute: appointment.StartTime.Minutes,
                        second: 0
                    ),
                EndTime = new DateTime(
                        year: appointment.ScheduleDate.Year,
                        month: appointment.ScheduleDate.Month,
                        day: appointment.ScheduleDate.Day,
                        hour: appointment.StartTime.Hours + appointment.RequestedHours.Hours,
                        minute: appointment.StartTime.Minutes,
                        second: 0
                    ),
                IsAllDay = false,
                Background = isScheduled ? Brush.Green
                                           : Brush.Gray,
                Notes = string.IsNullOrEmpty(appointment.Notes) ? "no notes" : appointment.Notes,
                Subject = isScheduled ? appointment.CleaningType
                                           : "Not Scheduled",
            };
            convertedAppointments.Add(schedAppointment);
        }
        Appointments = new ObservableCollection<SchedulerAppointment>(convertedAppointments);
    }

    private async Task CustomerSetup()
    {
        IsCleaner = false;
        var contracts = await _customerService.GetAllAppointments();
        foreach (var contract in contracts)
        {
            var isScheduled = await _customerService.IsScheduled(contract.Id);
            var appointment = new SchedulerAppointment
            {
                Id = contract.Id,
                StartTime = contract.ScheduleDate,
                EndTime = contract.ScheduleDate + contract.RequestedHours,
                IsAllDay = !isScheduled,
                Subject = contract?.CleaningType.Type,
                Background = isScheduled ? Brush.Green
                                           : Brush.Red,
                Notes = contract.Notes,                
            };
            Appointments.Add(appointment);
        }
    }

    [RelayCommand]
    public async Task ViewChanged(SchedulerViewChangedEventArgs e)
    {
        if (IsCleaner)
        { 
            await CleanerSetup(e.NewVisibleDates.Min(s => s.Date));
        }
    }
}
