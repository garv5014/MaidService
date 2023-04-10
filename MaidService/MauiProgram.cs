using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Supabase;
using MaidService.Views;
using MaidService.ViewModels;
using Syncfusion.Maui.Core.Hosting;
using Maid.Library.Interfaces;
using MaidService.Services;
using MaidService.CustomComponents;
using MaidService.ComponentsViewModels;
using MaidService.Mappers;

namespace MaidService;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.ConfigureSyncfusionCore();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        var url = DeviceInfo.Current.Platform == DevicePlatform.Android ? "http://10.0.2.2:8000" : "http://localhost:8000";
        var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.ewogICAgInJvbGUiOiAiYW5vbiIsCiAgICAiaXNzIjogInN1cGFiYXNlIiwKICAgICJpYXQiOiAxNjc4OTQ2NDAwLAogICAgImV4cCI6IDE4MzY3OTkyMDAKfQ.o8K_A6Yb58TmcKIcZWk-f36JdFM2z5mWhfDG5pnLLDw";
        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        };

        builder.Services.AddAutoMapper(typeof(MapperProfile));

        // Pages
        InitPages(builder);

        // ComponentViewModels
        builder.Services.AddSingleton<AppointmentCardViewModel>();
        // Components
        builder.Services.AddSingleton<AppointmentCard>();
        // Note the creation as a singleton.
        builder.Services.AddSingleton(new Supabase.Client(url, key, options: options));
        builder.Services.AddSingleton<ICustomerService, CustomerService>();
        builder.Services.AddSingleton<INavService, NavigationService>();
        builder.Services.AddSingleton<IAuthService, AuthenicationService>();
        builder.Services.AddSingleton<IPlatformService, MauiPlatformService>();

        InitViewModels(builder);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void InitPages(MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<CustomerProfile>();
        builder.Services.AddSingleton<OrderDetails>();
        builder.Services.AddSingleton<CustomerSchedule>();
        builder.Services.AddSingleton<SignUpPage>();
        builder.Services.AddSingleton<ScheduleForm>();
        builder.Services.AddSingleton<SignOutPage>();
    }

    public static void InitViewModels(MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<CustomerProfileViewModel>();
        builder.Services.AddSingleton<OrderDetailsViewModel>();
        builder.Services.AddSingleton<LoginPageViewModel>();
        builder.Services.AddSingleton<CustomerScheduleViewModel>();
        builder.Services.AddSingleton<SignUpPageViewModel>();

        builder.Services.AddSingleton<ScheduleFormViewModel>();
    }
}

