﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using System.Collections.ObjectModel;

namespace MaidService.ViewModels;

public partial class ScheduleFormViewModel : ObservableObject
{
    private ICustomerService _customerService;
    private IPlatformService _platform;

    public ScheduleFormViewModel(ICustomerService service, IPlatformService platform)
    {
        _customerService = service;
        _platform = platform;
    }

    [ObservableProperty]
    private string displayPrice;

    [ObservableProperty]
    private int selectedIndex = 0;

    [ObservableProperty]
    private CleaningContract contract = new() { ScheduleDate = DateTime.Now };

    [ObservableProperty]
    private ObservableCollection<CleaningType> cleaningTypes;

    [ObservableProperty]
    private ObservableCollection<FileResult> contractPhotos = new();

    private int requestedHours;

    public int RequestedHours
    {
        get
        {
            return requestedHours;
        }
        set
        {
            SetProperty(ref requestedHours, value);
            Contract.Cost = (value * 65).ToString();
            DisplayPrice = Contract.Cost;
        }
    }

    [ObservableProperty]
    private DateTime minDate = DateTime.Now + TimeSpan.FromDays(2);

    [RelayCommand]
    public async Task Appear()
    {
        CleaningTypes = new() { new CleaningType { Type = "Loading..." } };
        var tempTypes = new ObservableCollection<CleaningType>();
        var allTypes = await _customerService.GetCleaningTypes();
        if (allTypes != null)
        {
            foreach (var type in allTypes)
            {
                tempTypes.Add(new CleaningType
                {
                    Id = type.Id,
                    Type = type.Type,
                    Description = type.Description,
                });
            }
        }
        else
        {
            tempTypes.Add(new CleaningType
            {
                Id = 0,
                Type = "No Cleaning",
                Description = "No Availble types",
            });
        }
        CleaningTypes = tempTypes;
    }

    [RelayCommand]
    public async Task SelectPhotos()
    {
        var photo = await _platform.PickImageFile();
        if (photo != null)
        {
            ContractPhotos.Add(photo);
        }
    }

    [RelayCommand]
    public void RemoveImage(string test)
    {
        ContractPhotos.Remove(ContractPhotos.FirstOrDefault(x => x.FullPath == test));
    }

    [RelayCommand]
    public async Task AddJob()
    {
        // make sure the required fields are filled out
        if (!IsValidContract())
        {
            _platform.DisplayAlert("Missing Value In Fields", "Please make sure all the fields are filled in.", "Ok");
        }
        else
        {
            // if all fields are good make add the contract to the database
            // and show a success message to the user
            Contract.RequestedHours = TimeSpan.FromHours(RequestedHours);
            Contract.CleaningType = new CleaningType { Type = CleaningTypes[SelectedIndex].Type, Id = CleaningTypes[SelectedIndex].Id };
            try
            {
                await _customerService.CreateNewContract(Contract);

            }
            catch (Exception)
            {
                _platform.DisplayAlert("Error", "There was an issue submitting your form. Please try again later", "Ok");
                return;
            }
            _platform.DisplayAlert("All Done", "Your appoinment was scheduled", "Ok");
            ClearForm();
        }
    }

    private bool IsValidContract()
    {
        var res = !string.IsNullOrEmpty(Contract.Location.Address)
            && !string.IsNullOrEmpty(Contract.Location.City)
            && !string.IsNullOrEmpty(Contract.Location.State)
            && !string.IsNullOrEmpty(Contract.Location.ZipCode)
            && Contract.EstSqft > 0
            && RequestedHours >= 1
            && Contract.ScheduleDate > DateTime.Now;
        return res;
    }

    void ClearForm()
    {
        Contract.Location.Address = "";
        Contract.Location.City = "";
        Contract.Location.State = "";
        Contract.Location.ZipCode = "";
        Contract.EstSqft = 0;
        Contract.ScheduleDate = DateTime.Now;
        Contract.Cost = "0";
        Contract.Notes = "";
        RequestedHours = 0;
        OnPropertyChanged(nameof(Contract));
        OnPropertyChanged(nameof(RequestedHours));
    }
}