using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Maid.Library.Interfaces;
using MaidService.Library.DbModels;
using MaidService.Views;

namespace MaidService.ComponentsViewModels;

public partial class CustomerAppointmentCardViewModel : ObservableObject
{
    private readonly INavService _nav;
    private readonly ISupabaseStorage _supabaseStorage;

    [ObservableProperty]
    private CleaningContract cardContract;

    [ObservableProperty]
    private IEnumerable<Cleaner> profilePictures;

    public CustomerAppointmentCardViewModel(CleaningContract cleaningContract, INavService nav, ISupabaseStorage supabaseStorage)
    {
        CardContract = cleaningContract;
        _nav = nav;
        _supabaseStorage = supabaseStorage;
        ProfilePictures = _supabaseStorage.GetCleanersProfilePicturesFromAContract(CardContract);
    }

    [RelayCommand]
    public async Task TapCard()
    {
        await _nav.NavigateToWithParameters($"///{nameof(CustomerOrderDetails)}",
            new Dictionary<string, object>
            {
                ["Contract"] = CardContract
            }
            );  
    }
}
