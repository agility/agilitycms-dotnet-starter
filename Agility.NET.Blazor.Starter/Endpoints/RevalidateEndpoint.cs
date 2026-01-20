using Agility.Net.Blazor.Starter.Services.Cache;

namespace Agility.Net.Blazor.Starter.Endpoints;

/// <summary>
/// Request model for the revalidate webhook from Agility CMS.
/// This is called when content is published in Agility.
/// </summary>
public class RevalidateRequest
{
    public string State { get; set; } = string.Empty;
    public string InstanceGuid { get; set; } = string.Empty;
    public string LanguageCode { get; set; } = string.Empty;
    public string? ReferenceName { get; set; }
    public int? ContentID { get; set; }
    public int? ContentVersionID { get; set; }
    public int? PageID { get; set; }
    public int? PageVersionID { get; set; }
    public DateTime? ChangeDateUTC { get; set; }
}

/// <summary>
/// Minimal API endpoint for cache revalidation webhooks from Agility CMS.
/// Configure this URL in Agility CMS under Settings > Webhooks to invalidate
/// cached content when items are published.
/// </summary>
public static class RevalidateEndpoint
{
    public static void MapRevalidateEndpoint(this WebApplication app)
    {
        app.MapPost("/api/revalidate", async (
            RevalidateRequest request,
            IAgilityCacheService cacheService,
            ILogger<Program> logger) =>
        {
            logger.LogInformation(
                "Revalidate webhook received: State={State}, ReferenceName={ReferenceName}, ContentID={ContentID}, PageID={PageID}, Locale={Locale}",
                request.State, request.ReferenceName, request.ContentID, request.PageID, request.LanguageCode);

            // Only process publish events
            if (!string.Equals(request.State, "Published", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogDebug("Ignoring non-publish event: {State}", request.State);
                return Results.Ok(new { message = "OK", processed = false, reason = "Not a publish event" });
            }

            var tagsInvalidated = new List<string>();

            if (!string.IsNullOrEmpty(request.ReferenceName))
            {
                // Content item change - invalidate by reference name and content ID
                var listTag = AgilityCacheKeys.ContentList(request.ReferenceName, request.LanguageCode);
                cacheService.InvalidateTag(listTag);
                tagsInvalidated.Add(listTag);

                if (request.ContentID.HasValue && request.ContentID.Value > 0)
                {
                    var itemTag = AgilityCacheKeys.ContentItem(request.ContentID.Value, request.LanguageCode);
                    cacheService.InvalidateTag(itemTag);
                    tagsInvalidated.Add(itemTag);
                }

                // Also invalidate GraphQL cache for this reference name
                var graphqlTag = AgilityCacheKeys.GraphQL(request.ReferenceName, request.LanguageCode);
                // Invalidate all GraphQL tags that start with this prefix (handles pagination variants)
                foreach (var tag in cacheService.GetCachedTags().Where(t => t.StartsWith(graphqlTag)))
                {
                    cacheService.InvalidateTag(tag);
                    tagsInvalidated.Add(tag);
                }

                logger.LogInformation("Revalidated content tags: {Tags}", string.Join(", ", tagsInvalidated));
            }
            else if (request.PageID.HasValue && request.PageID.Value > 0)
            {
                // Page change - invalidate page and sitemaps
                var pageTag = AgilityCacheKeys.Page(request.PageID.Value, request.LanguageCode);
                cacheService.InvalidateTag(pageTag);
                tagsInvalidated.Add(pageTag);

                var sitemapFlatTag = AgilityCacheKeys.SitemapFlat(request.LanguageCode);
                var sitemapNestedTag = AgilityCacheKeys.SitemapNested(request.LanguageCode);
                cacheService.InvalidateTag(sitemapFlatTag);
                cacheService.InvalidateTag(sitemapNestedTag);
                tagsInvalidated.Add(sitemapFlatTag);
                tagsInvalidated.Add(sitemapNestedTag);

                // Also invalidate the combined sitemap cache
                foreach (var tag in cacheService.GetCachedTags().Where(t => t.Contains("agility-sitemap-flat")))
                {
                    cacheService.InvalidateTag(tag);
                    tagsInvalidated.Add(tag);
                }

                logger.LogInformation("Revalidated page and sitemap tags: {Tags}", string.Join(", ", tagsInvalidated));
            }
            else
            {
                // No content or page specified - this is a URL redirections change
                var urlRedirectionsTag = AgilityCacheKeys.UrlRedirections;
                cacheService.InvalidateTag(urlRedirectionsTag);
                tagsInvalidated.Add(urlRedirectionsTag);

                logger.LogInformation("Revalidated URL redirections tag: {Tag}", urlRedirectionsTag);
            }

            return Results.Ok(new
            {
                message = "OK",
                processed = true,
                tagsInvalidated = tagsInvalidated.Distinct().ToList()
            });
        })
        .WithName("Revalidate")
        .WithTags("Cache");

        // Optional: Add an endpoint to view cached tags (useful for debugging)
        app.MapGet("/api/cache/tags", (IAgilityCacheService cacheService) =>
        {
            return Results.Ok(new
            {
                cachedTags = cacheService.GetCachedTags()
            });
        })
        .WithName("GetCacheTags")
        .WithTags("Cache");

        // Optional: Add an endpoint to clear all cache
        app.MapPost("/api/cache/clear", (IAgilityCacheService cacheService, ILogger<Program> logger) =>
        {
            var count = cacheService.GetCachedTags().Count;
            cacheService.InvalidateAll();
            logger.LogInformation("Cleared all {Count} cache entries", count);

            return Results.Ok(new
            {
                message = "Cache cleared",
                entriesCleared = count
            });
        })
        .WithName("ClearCache")
        .WithTags("Cache");
    }
}
