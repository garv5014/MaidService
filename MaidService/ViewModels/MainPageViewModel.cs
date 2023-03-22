using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.DbModels;
using Supabase;

namespace MaidService.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
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
