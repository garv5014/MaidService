namespace Maid.Library.Interfaces;

public interface INavigation
{
    void NavigateTo(string path);
    void NavigateToWithParameters(string path, Dictionary<string, object> args);
}
