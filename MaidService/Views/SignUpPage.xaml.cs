using MaidService.ViewModels;

namespace MaidService.Views;

public partial class SignUpPage : ContentPage
{
	public SignUpPage(SignUpPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}