using Maid.Library.Interfaces;

namespace MaidService.Services;

public class MauiPlatformService : IPlatformService
{
    public async void DisplayAlert(string header, string message, string buttonText)
    {
        await Shell.Current.DisplayAlert(header, message, buttonText);
    }
}
