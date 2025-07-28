using Microsoft.Extensions.Caching.Memory;
using TrainComponentManagementSystem.Services.Interfaces;

namespace TrainComponentManagementSystem.Services
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly HashSet<string> _keys = new();

        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Set<T>(string key, T item, MemoryCacheEntryOptions options)
        {
            _cache.Set(key, item, options);
            lock (_keys)
            {
                _keys.Add(key);
            }
        }

        public bool TryGetValue<T>(string key, out T item)
        {
            return _cache.TryGetValue(key, out item);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
            lock (_keys)
            {
                _keys.Remove(key);
            }
        }

        public void RemoveByPrefix(string prefix)
        {
            lock (_keys)
            {
                var keysToRemove = _keys.Where(k => k.StartsWith(prefix)).ToList();
                foreach (var key in keysToRemove)
                {
                    _cache.Remove(key);
                    _keys.Remove(key);
                }
            }
        }
    }
}
