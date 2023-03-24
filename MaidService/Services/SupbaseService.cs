using Maid.Library.Interfaces;
using Postgrest.Models;
using Supabase;

namespace MaidService.Services;

public class SupbaseService : ISupabaseService
{
	public Supabase.Client Client { get; set; }

	public SupbaseService(string url, string key, SupabaseOptions? options = null)
	{
		Client = new Supabase.Client(url,key, options);
	}

    public async Task<MyModelResponse<T>> GetTable<T>() where T : BaseModel, new()
    {
		var res = await Client.Postgrest.Table<T>().Get();
        return  new MyModelResponse<T>(res);
    }
}
