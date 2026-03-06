
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace ServerlessApi.Utils;

public static class Cache
{
    private class CacheEntry
    {
        public APIGatewayHttpApiV2ProxyResponse Response { get; set; } = default!;
        public DateTime Timestamp { get; set; }
    }

    private static readonly ConcurrentDictionary<string, CacheEntry> _cache = new();

    public static Func<APIGatewayHttpApiV2ProxyRequest, ILambdaContext, Task<APIGatewayHttpApiV2ProxyResponse>> WithCache(
        string key,
        int ttlSeconds,
        Func<APIGatewayHttpApiV2ProxyRequest, ILambdaContext, Task<APIGatewayHttpApiV2ProxyResponse>> handler)
    {
        return async (request, context) =>
        {
            var now = DateTime.UtcNow;
            if (_cache.TryGetValue(key, out var entry))
            {
                if ((now - entry.Timestamp).TotalSeconds < ttlSeconds)
                {
                    entry.Response.Headers["X-Cache"] = "HIT";
                    return entry.Response;
                }
            }

            var response = await handler(request, context);
            response.Headers["X-Cache"] = "MISS";
            _cache[key] = new CacheEntry { Response = response, Timestamp = now };
            return response;
        };
    }
}
