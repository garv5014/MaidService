using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.Views;

namespace MaidService.ComponentsViewModels;

public partial class CleanerAppointmentCardViewModel : ObservableObject
{
    [ObservableProperty]
    private string address;

    [ObservableProperty]
    private string? cleaningDate;

    private readonly INavService _nav;

    public int ContractId { get; init; }

    public CleanerAppointmentCardViewModel(CleaningContract cleaningContract, INavService nav)
    {
        Address = cleaningContract.Location.Address;
        CleaningDate = cleaningContract.ScheduleDate.ToShortDateString();
        ContractId = cleaningContract.Id;
        _nav = nav;
    }

    [RelayCommand]
    public async Task NavigateToCleanerDetails()
    {
        await _nav.NavigateTo($"///{nameof(CleanerOrderDetails)}?ContractId={ContractId}");
    }
}
