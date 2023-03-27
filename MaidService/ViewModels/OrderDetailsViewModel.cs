using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;

namespace MaidService.ViewModels;

public partial class OrderDetailsViewModel : ObservableObject
{
	private ICustomerService _customer;

	public OrderDetailsViewModel(ICustomerService customer)
	{
		this._customer = customer;
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

	[RelayCommand]
	public async Task Appear()
	{
		var result = await _customer.GetCleaningDetailsById(1);

		if (result.Id > 0)
		{
			Price = $"${result.Cost}";
			ScheduledTime = result.ScheduleDate.ToShortTimeString();
			TypeOfCleaning = result.CleaningType.Type;
			Location = result.Location.Address;
			Notes = result.Notes;
		}
	}
}