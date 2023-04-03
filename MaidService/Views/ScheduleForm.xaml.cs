using MaidService.ViewModels;

namespace MaidService.Views;

public partial class ScheduleForm : ContentPage
{
	public ScheduleForm(ScheduleFormViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
    }
}