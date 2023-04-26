using MaidService.Library.Interfaces;
using System.Net.Http.Json;

namespace MaidService.Services;

public class ApiService : IApiService
{
    private readonly HttpClient version1;
    private readonly HttpClient version2;

    public ApiService(HttpClient v1, HttpClient v2)
    {
        version1 = v1;
        version2 = v2;
    }

    public async Task<string> GetLoginMessage()
    {
        //var res = version1.GetFromJsonAsync<string>("api/MaidService/LoginPage/Message");
        var res = await version2.GetAsync("api/MaidService/LoginPage/Message");
        return await res.Content.ReadAsStringAsync();
    }

    public async Task<string> GetImageUrl()
    {
        var result = await version1.GetAsync("api/MaidService/Logo");
        //var result = await version2.GetAsync("api/MaidService/Logo");
        return await result.Content.ReadAsStringAsync();
    }

    public async Task<int> GetFontSize()
    {
        var result = await version1.GetAsync("api/MaidService/LoginPage/FontSize");
        //var result = await version2.GetAsync("api/MaidService/LoginPage/FontSize");
        return await result.Content.ReadFromJsonAsync<int>();
    }
}
