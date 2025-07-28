using Microsoft.Extensions.Caching.Memory;

namespace TrainComponentManagementSystem.Services.Interfaces
{
    public interface ICacheService
    {
        void Set<T>(string key, T item, MemoryCacheEntryOptions options);
        bool TryGetValue<T>(string key, out T item);
        void Remove(string key);
        void RemoveByPrefix(string prefix);
    }
}