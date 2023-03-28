using Maid.Library.Interfaces;

namespace MaidService.Services;

public class NavigationService : INav
{
    public void NavigateTo(string path)
    {
        Shell.Current.GoToAsync(path);
    }

    public void NavigateToWithParameters(string path, Dictionary<string, object> args)
    {
        Shell.Current?.GoToAsync(path, args);
    }
}
