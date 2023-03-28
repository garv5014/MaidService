using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.Views;
using System.Security.Cryptography.X509Certificates;

namespace MaidService.ComponentsViewModels;

public partial class AppointmentCardViewModel : ObservableObject 
{
    [ObservableProperty]
    private string address;

    [ObservableProperty]
    private DateTime? cleaningDate;
    private readonly INav _nav;

    public int ContractId { get; init; }


    public AppointmentCardViewModel(CleaningContract cleaningContract, INav nav)
    {
        Address = cleaningContract.Location.Address;
        CleaningDate = cleaningContract.ScheduleDate;
        ContractId = cleaningContract.Id;
        _nav = nav;
    }

    [RelayCommand]
    public void TapCard()
    {
        _nav.NavigateToWithParameters($"///{nameof(OrderDetails)}", 
            new Dictionary<string, object> { { "contractId", ContractId } }
            );
    }
}
