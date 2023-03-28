using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.ComponentsViewModels;
using MaidService.Services;
using System.Collections.ObjectModel;

namespace MaidService.ViewModels;

public partial class CustomerScheduleViewModel : ObservableObject
{
    private ICustomerService customerService;

    public CustomerScheduleViewModel(ICustomerService customerService)
    {
        this.customerService = customerService;
    }

    public ObservableCollection<CleaningAppoinment> Appoinments { get; set; }

    [RelayCommand]
    public void Appear()
    {
        customerService
    }
}
