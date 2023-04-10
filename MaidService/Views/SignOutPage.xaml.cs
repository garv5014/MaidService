using Maid.Library.Interfaces;
using MaidService.ViewModels;

namespace MaidService.Views;

public partial class SignOutPage : ContentPage
{
    private readonly IAuthService auth;
    private readonly LoginPageViewModel vm;

    public SignOutPage(IAuthService auth, LoginPageViewModel vm)
	{
		InitializeComponent();
        this.auth = auth;
        this.vm = vm;
    }


    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        auth.SignOutUser();
        App.Current.MainPage = new LoginPage(vm);
    }
}