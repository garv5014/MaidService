using Postgrest.Models;

namespace Maid.Library.Interfaces;

public interface ISupabaseService
{
    Task<Postgrest.Responses.ModeledResponse<T>> GetTable<T>() where T : BaseModel, new();
}
