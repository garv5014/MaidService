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
        var translation = contracts.Select(x => new SchedulerAppointment
        {
            StartTime = x.ScheduleDate,
            EndTime = x.ScheduleDate + x.RequestedHours,
            IsAllDay = false, 
            Subject = x.CleaningType.Type,
            Background = Brush.Blue
        });
        foreach (var schedule in translation) 
        {
            Appointments.Add(schedule);
        }
    }
}
