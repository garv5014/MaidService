using Microsoft.Maui.Storage;

namespace Maid.Library.Interfaces;

public interface IPlatformService
{
    public void DisplayAlert(string header, string message, string buttonText);
    public Task<FileResult> PickFile(PickOptions options = null);
}