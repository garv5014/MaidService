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
    [ObservableProperty]
    private Customer currentCustomer = new();

    [ObservableProperty]
    private string selectedCleaning;

    public CustomerScheduleViewModel(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [ObservableProperty]
    private ObservableCollection<SchedulerAppointment> appointments;

    [RelayCommand]
    public async Task Appear()
    {
        Appointments = new();
        CurrentCustomer = await _customerService.GetCurrentCustomer();

        var contracts = await _customerService.GetAllAppointments(CurrentCustomer.Id);
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
