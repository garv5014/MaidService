using MaidService.ViewModels;

namespace MaidService.Views;

public partial class CleanerAcceptsShifts : ContentPage
{
	public CleanerAcceptsShifts(CleanerAcceptsShiftsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}