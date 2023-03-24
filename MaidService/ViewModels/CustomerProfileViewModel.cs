﻿using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.ComponentsViewModels;
using MaidService.DbModels;
using System.Collections.ObjectModel;

namespace MaidService.ViewModels;

public partial class CustomerProfileViewModel : ObservableObject
{
    private readonly ISupabaseService _supabase;


    [ObservableProperty]
    private IEnumerable<AppointmentCardViewModel> appointments;

    [ObservableProperty]
    private string appointmentsHeader = "No Upcoming Appointments";

    public CustomerProfileViewModel(ISupabaseService supabase)
    {
        _supabase = supabase;
    }

    [RelayCommand]
    public async Task Appear()
    {
        var res = await _supabase.GetTable<CleaningContract>();
        Appointments = res.Models.Select(a => new AppointmentCardViewModel(a));

        if (res.Models != null)
        {
            AppointmentsHeader = "Upcoming Appointments";
        }
    }
}
