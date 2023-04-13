using MaidService.ViewModels;
using MaidService.Views;

namespace MaidService;

public partial class App : Application
{
	public App(LoginPageViewModel vm)
	{
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM5NzQ2MkAzMjMwMmUzNDJlMzBJL1RPV09obVBvY0o1QXAzTmJjNVY5ODZIcVFtcTMzOG5zR20vUko2WG5rPQ==");
        InitializeComponent();

		MainPage = new AppShell();
	}
}
