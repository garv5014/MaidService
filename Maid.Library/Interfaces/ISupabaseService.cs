using Postgrest.Models;

namespace Maid.Library.Interfaces;

public interface ISupabaseService
{
    Task<MyModelResponse<T>> GetTable<T>() where T : BaseModel, new();
}
