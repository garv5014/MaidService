using MaidService.ViewModels;

namespace MaidService.Views;

public partial class CustomerSchedule : ContentPage
{
	public CustomerSchedule(CustomerScheduleViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}