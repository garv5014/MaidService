using MaidService.ComponentsViewModels;

namespace MaidService.CustomComponents;

public partial class Appointments
{
	public Appointments(AppointmentsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}