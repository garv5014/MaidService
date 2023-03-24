using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.DbModels;
using System.Reactive.Linq;

namespace MaidService.ComponentsViewModels;

public partial class AppointmentCardViewModel : ObservableObject 
{
    [ObservableProperty]
    private string address;

    [ObservableProperty]
    private int value; 

    private ISupabaseService supabaseService;
    public AppointmentCardViewModel(CleaningContract cleaningContract )
    {
        this.supabaseService = MauiProgram.Services.GetRequiredService<ISupabaseService>();
        Address = cleaningContract.Location.Address;
    }
    [RelayCommand]
    public void Increment()
    {
        Value++;
    }
    

}
