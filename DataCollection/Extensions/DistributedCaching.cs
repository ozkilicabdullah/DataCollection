using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace DataCollection.Extensions
{
    public static class DistributedCaching
    {

        public static async Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value, DateTime Expiration)
        {
            string serializedObject = JsonConvert.SerializeObject(value);
            await  distributedCache.SetStringAsync(key, serializedObject, new DistributedCacheEntryOptions() { AbsoluteExpiration = new DateTimeOffset(Expiration) });
        }

        public static async Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key) where T : class
        {
            string serializedObject = await distributedCache.GetStringAsync(key);

            if (!string.IsNullOrEmpty(serializedObject))
                return JsonConvert.DeserializeObject<T>(serializedObject);

            return null;            
        }

    }

}
