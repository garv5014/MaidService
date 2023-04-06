namespace Maid.Library.Interfaces;

public interface INavService
{
    Task NavigateTo(string path);
    Task NavigateToWithParameters(string path, Dictionary<string, object> args);
}
