using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;

namespace MaidService.ViewModels;

public partial class CustomerScheduleViewModel : ObservableObject
{
    private ICustomerService customerService;

    public CustomerScheduleViewModel(ICustomerService customerService)
    {
        this.customerService = customerService;
    }
    [ObservableProperty]
    private ObservableCollection<SchedulerAppointment> appointments;

    [RelayCommand]
    public async Task Appear()
    {
        Appointments = new();

        var contracts = await customerService.GetAllAppointments(1);
        foreach (var schedule in contracts)
        {
            Appointments.Add(new SchedulerAppointment
            {
                Id = schedule.Id,
                StartTime = schedule.ScheduleDate,
                EndTime = schedule.ScheduleDate + schedule.RequestedHours,
                IsAllDay = false,
                Subject = schedule?.CleaningType.Type,
                Background = await customerService.IsScheduled(schedule.Id) ?
                                                 Brush.Green
                                                :Brush.Red,
            });
        }
    }
}
