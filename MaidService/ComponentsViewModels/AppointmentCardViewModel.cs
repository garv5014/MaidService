using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaidService.Library.DbModels;
using System.Security.Cryptography.X509Certificates;

namespace MaidService.ComponentsViewModels;

public partial class AppointmentCardViewModel : ObservableObject 
{
    [ObservableProperty]
    private string address;

    [ObservableProperty]
    private DateTime? cleaningDate;

    public int ContractId { get; init; }


    public AppointmentCardViewModel(CleaningContract cleaningContract)
    {
        Address = cleaningContract.Location.Address;
        CleaningDate = cleaningContract.ScheduleDate;
        ContractId = cleaningContract.Id;
    }
}
