namespace MaidService.Library.Interfaces;

public interface IApiService
{
    public Task<string> GetLoginMessage();
    public Task<string> GetImageUrl();
    public Task<int> GetFontSize();
}
