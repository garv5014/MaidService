using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Views;

namespace MaidService.ViewModels;
[QueryProperty(nameof(ContractId), nameof(ContractId))]
public partial class OrderDetailsViewModel : ObservableObject
{
	private ICustomerService _customer;
	private INav _navService;

	public OrderDetailsViewModel(ICustomerService customer, INav navService)
	{
		_customer = customer;
		_navService = navService;
	}

	[ObservableProperty]
	private string cleanerName = null;

	[ObservableProperty]
	private string price = null;

	[ObservableProperty]
	private string scheduledTime = null;

	[ObservableProperty]
	private string typeOfCleaning = null;

	[ObservableProperty]
	private string location = null;

	[ObservableProperty]
	private string notes = null;

    [ObservableProperty]
	private int contractId ;

    [RelayCommand]
	public async Task NavigatedTo()
	{
		var result = await _customer.GetCleaningDetailsById(ContractId);

		if (result?.Id > 0)
		{
			Price = $"{result.Cost}";
			ScheduledTime = result.ScheduleDate.ToShortTimeString();
			TypeOfCleaning = result.CleaningType.Type;
			Location = result.Location.Address;
			Notes = result.Notes;
			CleanerName = result?.Cleaners?.First()?.FirstName;
		}
	}

	[RelayCommand]
	public async Task GoBack()
	{
		await _navService.NavigateTo($"///{nameof(CustomerProfile)}");
	}
}