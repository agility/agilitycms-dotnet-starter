namespace Agility.NET.Blazor.Starter.Services.Cache;

/// <summary>
/// Service for centralized caching of Agility CMS data with tag-based invalidation support.
/// </summary>
public interface IAgilityCacheService
{
    /// <summary>
    /// Gets a cached item or creates it using the factory function.
    /// </summary>
    /// <typeparam name="T">Type of the cached item</typeparam>
    /// <param name="cacheTag">The cache tag/key for this item</param>
    /// <param name="factory">Function to create the item if not cached</param>
    /// <param name="expirationMinutes">Optional expiration time override</param>
    /// <returns>The cached or newly created item</returns>
    Task<T> GetOrCreateAsync<T>(string cacheTag, Func<Task<T>> factory, int? expirationMinutes = null);

    /// <summary>
    /// Invalidates all cache entries matching the specified tag.
    /// </summary>
    /// <param name="cacheTag">The cache tag to invalidate</param>
    void InvalidateTag(string cacheTag);

    /// <summary>
    /// Invalidates multiple cache tags at once.
    /// </summary>
    /// <param name="cacheTags">The cache tags to invalidate</param>
    void InvalidateTags(IEnumerable<string> cacheTags);

    /// <summary>
    /// Invalidates all Agility CMS cache entries.
    /// </summary>
    void InvalidateAll();

    /// <summary>
    /// Gets all currently cached tags (for debugging/monitoring).
    /// </summary>
    IReadOnlyCollection<string> GetCachedTags();
}
