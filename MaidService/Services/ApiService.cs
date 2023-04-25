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
        return await version1.GetFromJsonAsync<string>("api/MaidService/LoginPage/Message");
    }
}
