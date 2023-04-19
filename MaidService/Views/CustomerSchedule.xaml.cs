using MaidService.ViewModels;
using Syncfusion.Maui.Scheduler;

namespace MaidService.Views;

public partial class CustomerSchedule : ContentPage
{
    private readonly CustomerScheduleViewModel vm;

    public CustomerSchedule(CustomerScheduleViewModel vm)
    {
        InitializeComponent();
        this.vm = vm;
        BindingContext = vm;
        this.Scheduler.ViewChanged += this.OnSchedulerViewChanged;
    }  

    private async void OnSchedulerViewChanged(object sender, SchedulerViewChangedEventArgs e)
    {
        vm.ViewChangedCommand.Execute(e);
    }
}