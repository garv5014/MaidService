using MaidService.ViewModels;

namespace MaidService.Views;

public partial class CleanerOrderDetails : ContentPage
{
	public CleanerOrderDetails(CleanerOrderDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}