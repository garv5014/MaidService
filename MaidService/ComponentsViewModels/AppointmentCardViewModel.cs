using CommunityToolkit.Mvvm.ComponentModel;
using System.Reactive.Linq;

namespace MaidService.ComponentsViewModels;

public partial class AppointmentCardViewModel : ObservableObject 
{
    [ObservableProperty]
    private string address;

    public AppointmentCardViewModel()
    {
        Address = "Fake Location";
    }

}
