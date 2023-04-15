using CommunityToolkit.Mvvm.ComponentModel;
using Maid.Library.Interfaces;

namespace MaidService.ViewModels;

public partial class CleanerAddAppointmentViewModel : ObservableObject
{
    private ICleanerService _cleanerService;

    public CleanerAddAppointmentViewModel(ICleanerService cleanerService)
    {
        _cleanerService = cleanerService;
    }

}
