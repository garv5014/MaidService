using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Supabase.Interfaces;
using Supabase;
using MaidService.Views;
using MaidService.ViewModels;
using Syncfusion.Maui.Core.Hosting;
using Maid.Library.Interfaces;
using MaidService.Services;
using MaidService.CustomComponents;
using MaidService.ComponentsViewModels;

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
        // Pages
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<CustomerProfile>();
        // ComponentViewModels
        builder.Services.AddSingleton<AppointmentCardViewModel>();
        // Components
        builder.Services.AddSingleton<AppointmentCard>();
        InitViewModels(builder);
        // Note the creation as a singleton.
        builder.Services.AddSingleton<ISupabaseService>(provider => new SupbaseService(url, key, options));
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
    public static void InitViewModels(MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<CustomerProfileViewModel>();
    }
}

