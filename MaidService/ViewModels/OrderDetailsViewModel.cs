using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.DbModels;

namespace MaidService.ViewModels;

public partial class OrderDetailsViewModel : ObservableObject
{
	private ISupabaseService supabase;

	public OrderDetailsViewModel(ISupabaseService supabase)
	{
		this.supabase = supabase;
	}

	[ObservableProperty]
	private string cleanerName;

	[ObservableProperty]
	private string price;

	[ObservableProperty]
	private string scheduledTime;

	[ObservableProperty]
	private string typeOfCleaning;

	[ObservableProperty]
	private string location;

	[ObservableProperty]
	private string notes;

	[RelayCommand]
	public async Task Appear()
	{
		var result = await supabase.GetTable<CleaningContract>();

		if (result != null)
		{
			Price = $"${result.Models.Select(x => x.Cost)}";
			ScheduledTime = result.Models.Select(x => x.ScheduleDate).First().ToShortTimeString();
			TypeOfCleaning = result.Models.Select(x => x.CleaningType.Type).First();
			Location = result.Models.Select(x => x.Location.Address).First();
			Notes = result.Models.Select(x => x.Notes).First();
		}
	}
}