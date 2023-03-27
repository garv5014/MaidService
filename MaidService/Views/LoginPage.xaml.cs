using MaidService.ViewModels;

namespace MaidService.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel vm )
	{
		InitializeComponent();
		BindingContext = vm;
	}
}