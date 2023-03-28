namespace Maid.Library.Interfaces;

public interface INav
{
    void NavigateTo(string path);
    void NavigateToWithParameters(string path, Dictionary<string, object> args);
}
