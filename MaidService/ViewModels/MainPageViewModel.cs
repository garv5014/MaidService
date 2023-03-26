using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.ComponentsViewModels;
using MaidService.DbModels;
using Supabase;

namespace MaidService.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    public AppointmentCardViewModel TestAppointment { get; set; } = new AppointmentCardViewModel { Address = "This is just a drill" };

    [ObservableProperty]
    private string text;

    public MainPageViewModel(ISupabaseService client)
    {
        Client = client;
    }

    public ISupabaseService Client { get; }

    [RelayCommand]
    public async Task GetCustomers()
    {
        var res = await Client.GetTable<Customer>();
        if (res == null)
        {

        }
        else
        {
            Text = res.Content;
        }
    }

}
