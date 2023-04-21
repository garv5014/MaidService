using Microsoft.Maui.Storage;

namespace Maid.Library.Interfaces;

public interface IPlatformService
{
    public void DisplayAlert(string header, string message, string buttonText);
    public Task<FileResult> PickImageFile(PickOptions options = null);
    Task<IEnumerable<FileResult>> PickMultipleFilesImages(PickOptions options = null);
}