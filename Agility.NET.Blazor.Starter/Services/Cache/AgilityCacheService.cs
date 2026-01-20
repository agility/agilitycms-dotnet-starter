using System.Collections.Concurrent;
using Agility.NET.FetchAPI.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Agility.Net.Blazor.Starter.Services.Cache;

/// <summary>
/// Centralized caching service for Agility CMS data with tag-based invalidation support.
/// This service allows for fine-grained cache invalidation via webhooks when content is published.
/// </summary>
public class AgilityCacheService : IAgilityCacheService
{
    private readonly IMemoryCache _cache;
    private readonly AppSettings _appSettings;
    private readonly ILogger<AgilityCacheService> _logger;

    // Track all cache tags for invalidation purposes
    private readonly ConcurrentDictionary<string, byte> _cacheTags = new();

    public AgilityCacheService(
        IMemoryCache cache,
        IOptions<AppSettings> appSettings,
        ILogger<AgilityCacheService> logger)
    {
        _cache = cache;
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<T> GetOrCreateAsync<T>(string cacheTag, Func<Task<T>> factory, int? expirationMinutes = null)
    {
        // Try to get from cache first
        if (_cache.TryGetValue(cacheTag, out T? cachedValue) && cachedValue != null)
        {
            _logger.LogDebug("Cache hit for tag: {CacheTag}", cacheTag);
            return cachedValue;
        }

        // Create the item
        _logger.LogDebug("Cache miss for tag: {CacheTag}, fetching from API", cacheTag);
        var value = await factory();

        if (value != null)
        {
            var expiration = expirationMinutes ?? _appSettings.CacheInMinutes;
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(expiration),
                Priority = CacheItemPriority.Normal
            };

            _cache.Set(cacheTag, value, cacheOptions);
            _cacheTags.TryAdd(cacheTag, 0);
        }

        return value;
    }

    /// <inheritdoc />
    public void InvalidateTag(string cacheTag)
    {
        _logger.LogInformation("Invalidating cache tag: {CacheTag}", cacheTag);
        _cache.Remove(cacheTag);
        _cacheTags.TryRemove(cacheTag, out _);
    }

    /// <inheritdoc />
    public void InvalidateTags(IEnumerable<string> cacheTags)
    {
        foreach (var tag in cacheTags)
        {
            InvalidateTag(tag);
        }
    }

    /// <inheritdoc />
    public void InvalidateAll()
    {
        _logger.LogInformation("Invalidating all Agility cache entries ({Count} tags)", _cacheTags.Count);

        foreach (var tag in _cacheTags.Keys)
        {
            _cache.Remove(tag);
        }

        _cacheTags.Clear();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<string> GetCachedTags()
    {
        return _cacheTags.Keys.ToList().AsReadOnly();
    }
}
