namespace Maid.Library.Interfaces;

public interface INav
{
    Task NavigateTo(string path);
    Task NavigateToWithParameters(string path, Dictionary<string, object> args);
}
