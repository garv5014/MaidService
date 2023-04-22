using Maid.Library.Interfaces;

namespace MaidService.Services;

public class MauiPlatformService : IPlatformService
{
    public async void DisplayAlert(string header, string message, string buttonText)
    {
        await Shell.Current.DisplayAlert(header, message, buttonText);
    }

    public async Task<FileResult> PickImageFile(PickOptions options = null)
    {
        try
        {
            if (options == null)
            { 
                options = PickOptions.Images;
            }
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    using var stream = await result.OpenReadAsync();
                    var image = ImageSource.FromStream(() => stream);
                }
                return result;
            }
        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
        }

        return null;
    }

    public async Task<IEnumerable<FileResult>> PickMultipleFilesImages(PickOptions options = null)
    {
        try
        {
            var result = await FilePicker.Default.PickMultipleAsync(PickOptions.Images);
            if (result != null)
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
        }
        return null;
    }
}
