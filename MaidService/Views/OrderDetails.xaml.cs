using MaidService.ViewModels;

namespace MaidService.Views;

public partial class OrderDetails : ContentPage
{
	public OrderDetails(OrderDetailsViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}