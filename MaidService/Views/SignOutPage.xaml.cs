using Maid.Library.Interfaces;
using MaidService.ViewModels;

namespace MaidService.Views;

public partial class SignOutPage : ContentPage
{
    private readonly IAuthService auth;
    private readonly LoginPageViewModel vm;
    private readonly INavService nav;

    public SignOutPage(IAuthService auth, LoginPageViewModel vm, INavService nav)
	{
		InitializeComponent();
        this.auth = auth;
        this.vm = vm;
        this.nav = nav;
    }


    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        auth.SignOutUser();
        await nav.NavigateTo($"///{nameof(LoginPage)}");
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
    }
}