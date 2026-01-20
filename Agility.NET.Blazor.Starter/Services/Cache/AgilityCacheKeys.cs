namespace Agility.NET.Blazor.Starter.Services.Cache;

/// <summary>
/// Centralized cache key/tag generation for Agility CMS data.
/// These tags can be used to invalidate specific cached content when
/// content is published via the revalidate webhook endpoint.
/// </summary>
public static class AgilityCacheKeys
{
    /// <summary>
    /// Cache tag for a specific content item by ID.
    /// Example: "agility-content-123-en-us"
    /// </summary>
    public static string ContentItem(int contentId, string locale) =>
        $"agility-content-{contentId}-{locale}";

    /// <summary>
    /// Cache tag for a content list by reference name.
    /// Example: "agility-content-posts-en-us"
    /// </summary>
    public static string ContentList(string referenceName, string locale) =>
        $"agility-content-{referenceName}-{locale}";

    /// <summary>
    /// Cache tag for a page by ID.
    /// Example: "agility-page-42-en-us"
    /// </summary>
    public static string Page(int pageId, string locale) =>
        $"agility-page-{pageId}-{locale}";

    /// <summary>
    /// Cache tag for the flat sitemap.
    /// Example: "agility-sitemap-flat-en-us"
    /// </summary>
    public static string SitemapFlat(string locale) =>
        $"agility-sitemap-flat-{locale}";

    /// <summary>
    /// Cache tag for the nested sitemap.
    /// Example: "agility-sitemap-nested-en-us"
    /// </summary>
    public static string SitemapNested(string locale) =>
        $"agility-sitemap-nested-{locale}";

    /// <summary>
    /// Cache tag for URL redirections.
    /// </summary>
    public static string UrlRedirections => "agility-url-redirections";

    /// <summary>
    /// Cache tag for GraphQL queries by reference name.
    /// Example: "agility-graphql-posts-en-us"
    /// </summary>
    public static string GraphQL(string referenceName, string locale) =>
        $"agility-graphql-{referenceName}-{locale}";
}
