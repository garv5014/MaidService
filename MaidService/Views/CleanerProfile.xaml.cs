using MaidService.ViewModels;

namespace MaidService.Views;

public partial class CleanerProfile : ContentPage
{
	public CleanerProfile(CleanerProfileViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}