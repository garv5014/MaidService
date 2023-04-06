using Maid.Library.Interfaces;

namespace MaidService.Services;

public class NavigationService : INavService
{
    public async Task NavigateTo(string path)
    {
        await Shell.Current.GoToAsync(path);
    }

    public async Task NavigateToWithParameters(string path, Dictionary<string, object> args)
    {
        await Shell.Current?.GoToAsync(path, args);
    }
}
