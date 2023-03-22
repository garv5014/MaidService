using MaidService.ViewModels;

namespace MaidService.Views;

public partial class CustomerProfile : ContentPage
{
	public CustomerProfile(CustomerProfileViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}