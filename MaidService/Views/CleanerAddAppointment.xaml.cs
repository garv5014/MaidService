using MaidService.ViewModels;

namespace MaidService.Views;

public partial class CleanerAddAppointment : ContentPage
{
	public CleanerAddAppointment(CleanerAddAppointmentViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}