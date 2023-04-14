using MaidService.ViewModels;

namespace MaidService.Views;

public partial class AvailableCleanerAppointments : ContentPage
{
	public AvailableCleanerAppointments(AvailableCleanerAppointmentsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}