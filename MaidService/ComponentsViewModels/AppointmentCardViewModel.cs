using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaidService.Library.DbModels;

namespace MaidService.ComponentsViewModels;

public partial class AppointmentCardViewModel : ObservableObject 
{
    [ObservableProperty]
    private string address;

    [ObservableProperty]
    private int value; 


    public AppointmentCardViewModel(CleaningContract cleaningContract)
    {
        Address = cleaningContract.Location.Address;
    }
    public AppointmentCardViewModel()
    {
        Address = "Fake Location";
    }

    [RelayCommand]
    public void Increment()
    {
        Value++;
    }
    

}
