﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.ComponentsViewModels;
using MaidService.Library.DbModels;

namespace MaidService.ViewModels;

public partial class CleanerProfileViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;
    private readonly INavService _nav;
    private readonly ICleanerService _cleanerService;

    [ObservableProperty]
    private Cleaner currentCleaner = new();

    [ObservableProperty]
    private CleaningContract cleanerContract;

    [ObservableProperty]
    private IEnumerable<AppointmentCardViewModel> appointments;

    [ObservableProperty]
    private string appointmentsHeader;

    [ObservableProperty]
    private bool isEditing = false;

    [ObservableProperty]
    private bool isNotEditing = true;

    [ObservableProperty]
    private string bioText;

    public CleanerProfileViewModel(ICustomerService customerService, INavService nav, ICleanerService cleanerService)
    {
        _customerService = customerService;
        _nav = nav;
        _cleanerService = cleanerService;
        AppointmentsHeader = "No Upcoming Appointments";
    }

    [RelayCommand]
    public async Task Appear()
    {
        CurrentCleaner = await _cleanerService.GetCurrentCleaner();
        BioText = CurrentCleaner.Bio;
        var res = await _customerService.GetUpcomingAppointments(CurrentCleaner.Id);
        if (res != null)
        {
            Appointments = res.Select(a => new AppointmentCardViewModel(a, _nav));
        }

        if (Appointments.Count() > 0)
        {
            AppointmentsHeader = "Upcoming Appointments";
        }
    }

    [RelayCommand]
    public async Task EditBio()
    {
        ToggleEditing();
    }


    [RelayCommand]
    public async Task UpdateBio()
    {
        ToggleEditing();

        await _cleanerService.UpdateCleanerBio(BioText);
        await Appear();
    }
    private void ToggleEditing()
    {
        IsEditing = !IsEditing;
        IsNotEditing = !IsNotEditing;
    }
}