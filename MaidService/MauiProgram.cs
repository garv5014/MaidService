using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Supabase;
using Microsoft.Extensions.Configuration;
using MaidService.Views;
using MaidService.ViewModels;
using Syncfusion.Maui.Core.Hosting;
using Maid.Library.Interfaces;
using MaidService.Services;
using MaidService.CustomComponents;
using MaidService.ComponentsViewModels;
using MaidService.Mappers;
using System.Reflection;
using static System.Net.WebRequestMethods;
using MaidService.Library.Interfaces;
using System.Net.Http;

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

        LoadAppsettingsIntoConfig(builder);
        string url = DeviceInfo.Current.Platform == DevicePlatform.Android ? builder.Configuration["AndroidDevelopment"] : builder.Configuration["WindowDevelopment"];

        var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.ewogICAgInJvbGUiOiAiYW5vbiIsCiAgICAiaXNzIjogInN1cGFiYXNlIiwKICAgICJpYXQiOiAxNjc4OTQ2NDAwLAogICAgImV4cCI6IDE4MzY3OTkyMDAKfQ.o8K_A6Yb58TmcKIcZWk-f36JdFM2z5mWhfDG5pnLLDw";
        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true,           
        };

        builder.Services.AddAutoMapper(typeof(MapperProfile));

        builder.Services.AddHttpClient("v1", c =>
        {
            c.BaseAddress = new Uri("https://localhost:7203/");
            c.DefaultRequestHeaders.Add("version", "1.0");
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler());

        builder.Services.AddHttpClient("v2", c =>
        {
            c.BaseAddress = new Uri("https://localhost:7203/");
            c.DefaultRequestHeaders.Add("version", "2.0");
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler());

        builder.Services.AddSingleton<IApiService>(provider =>
        {
            var clientV1 = provider.GetRequiredService<IHttpClientFactory>().CreateClient("v1");
            var clientV2 = provider.GetRequiredService<IHttpClientFactory>().CreateClient("v2");

            return new ApiService(clientV1, clientV2);
        });

        // Pages
        InitPages(builder);

        // ComponentViewModels
        builder.Services.AddSingleton<CustomerAppointmentCardViewModel>();
        builder.Services.AddSingleton<CleanerAppointmentCardViewModel>();

        // Components
        builder.Services.AddSingleton<CustomerAppointmentCard>();
        builder.Services.AddSingleton<CleanerAppointmentCards>();

        // Note the creation as a singleton.
        builder.Services.AddSingleton(new Supabase.Client(url, key, options: options));
        builder.Services.AddSingleton<ICustomerService, CustomerService>();
        builder.Services.AddSingleton<ICleanerService, CleanerService>();
        builder.Services.AddSingleton<INavService, NavigationService>();
        builder.Services.AddSingleton<IAuthService, AuthenicationService>();
        builder.Services.AddSingleton<IPlatformService, MauiPlatformService>();
        builder.Services.AddSingleton<ISupabaseStorage, MaidStorage>();


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
        builder.Services.AddSingleton<CleanerProfile>();
        builder.Services.AddSingleton<CustomerOrderDetails>();
        builder.Services.AddSingleton<CleanerOrderDetails>();
        builder.Services.AddSingleton<CustomerSchedule>();
        builder.Services.AddSingleton<SignUpPage>();
        builder.Services.AddSingleton<ScheduleForm>();
        builder.Services.AddSingleton<SignOutPage>();
        builder.Services.AddSingleton<AvailableCleanerAppointments>();
        builder.Services.AddSingleton<CleanerAcceptsShifts>();
        builder.Services.AddSingleton<CleanerAddAppointment>();
    }

    private static void LoadAppsettingsIntoConfig(MauiAppBuilder builder)
    {
        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("MaidService.appsetting.json");

        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();

        builder.Configuration.AddConfiguration(config);
    }

    public static void InitViewModels(MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<CustomerProfileViewModel>();
        builder.Services.AddSingleton<CleanerProfileViewModel>();
        builder.Services.AddSingleton<CustomerOrderDetailsViewModel>();
        builder.Services.AddSingleton<CleanerOrderDetailsViewModel>();
        builder.Services.AddSingleton<LoginPageViewModel>();
        builder.Services.AddSingleton<CustomerScheduleViewModel>();
        builder.Services.AddSingleton<SignUpPageViewModel>();
        builder.Services.AddSingleton<ScheduleFormViewModel>();
        builder.Services.AddSingleton<AvailableCleanerAppointmentsViewModel>();
        builder.Services.AddSingleton<CleanerAcceptsShiftsViewModel>();
        builder.Services.AddSingleton<CleanerAddAppointmentViewModel>();
    }
}

