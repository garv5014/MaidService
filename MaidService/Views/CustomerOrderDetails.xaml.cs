using MaidService.ViewModels;

namespace MaidService.Views;

public partial class CustomerOrderDetails : ContentPage
{
	public CustomerOrderDetails(CustomerOrderDetailsViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}