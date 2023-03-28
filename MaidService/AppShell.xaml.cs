using MaidService.ViewModels;
using MaidService.Views;

namespace MaidService;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(OrderDetails), typeof(OrderDetailsViewModel));
	}
}
