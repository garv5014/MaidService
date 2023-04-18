using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.Views;
using System.Diagnostics.Contracts;

namespace MaidService.ComponentsViewModels;

public partial class CleanerAppointmentCardViewModel : ObservableObject
{
    public CleaningContract Contract { get; set; }

    private readonly INavService _nav;

    public CleanerAppointmentCardViewModel(CleaningContract cleaningContract, INavService nav)
    {
        Contract = cleaningContract;
        _nav = nav;
    }

    [RelayCommand]
    public async Task NavigateToCleanerDetails()
    {
        await _nav.NavigateToWithParameters(
            $"///{nameof(CleanerOrderDetails)}",
            new Dictionary<string, object> { {"Contract", Contract } }
        );
    }

    [RelayCommand]
    public async Task NavigateToAddAppointment()
    {
        await _nav.NavigateToWithParameters(
            $"///{nameof(CleanerAddAppointment)}",
            new Dictionary<string, object> { { "Contract", Contract } }
        );
    }
}
