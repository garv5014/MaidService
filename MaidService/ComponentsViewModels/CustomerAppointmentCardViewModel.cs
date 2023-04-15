using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.Views;

namespace MaidService.ComponentsViewModels;

public partial class CustomerAppointmentCardViewModel : ObservableObject
{
    [ObservableProperty]
    private string address;

    [ObservableProperty]
    private DateTime? cleaningDate;

    private readonly INavService _nav;

    private CleaningContract contract { get; set; }

    public int ContractId { get; init; }

    public CustomerAppointmentCardViewModel(CleaningContract cleaningContract, INavService nav)
    {
        contract = cleaningContract;
        Address = cleaningContract.Location.Address;
        CleaningDate = cleaningContract.ScheduleDate;
        ContractId = cleaningContract.Id;
        _nav = nav;
    }

    [RelayCommand]
    public async Task TapCard()
    {
        await _nav.NavigateToWithParameters($"///{nameof(CustomerOrderDetails)}",
            new Dictionary<string, object>
            {
                ["Contract"] = contract
            }
            );  
    }
}
