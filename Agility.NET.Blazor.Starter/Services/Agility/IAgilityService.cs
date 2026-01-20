using Agility.NET.Blazor.Starter.Models;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;

namespace Agility.NET.Blazor.Starter.Services.Agility;

public interface IAgilityService
{
    Task<List<SitemapPage>> GetSitemapPagesAsync();
    Task<SitemapPage?> GetPageByPathAsync(string path);
    Task<PageResponse?> GetPageAsync(int pageId, string locale);
    Task<ContentItemResponse<T>?> GetContentItemAsync<T>(int contentId, string locale, int contentLinkDepth = 0) where T : class;
    Task<List<T>> GetContentListAsync<T>(string referenceName, string locale, int take = 50, int contentLinkDepth = 0) where T : class;
    Task<List<ContentItemResponse<T>>> GetContentByGraphQLAsync<T>(string query, string objName, string locale) where T : class;
    Task<UrlRedirectionsResponse?> GetUrlRedirectsAsync();
    SitemapPage? CheckLocaleWithDifferentPageName(string path, List<SitemapPage> sitemapPages);
    bool IsPreviewMode { get; }
    void SetPreviewMode(bool isPreview);

    /// <summary>
    /// Gets posts using the GraphQL API. This is the recommended approach for fetching posts
    /// as it allows for precise field selection and efficient data retrieval.
    /// </summary>
    /// <param name="locale">The locale to fetch posts for (e.g., "en-us")</param>
    /// <param name="take">Number of posts to fetch</param>
    /// <param name="skip">Number of posts to skip (for pagination)</param>
    /// <returns>List of PostDisplayItem objects</returns>
    Task<List<PostDisplayItem>> GetPostsGraphQLAsync(string locale, int take = 10, int skip = 0);

    /// <summary>
    /// Gets posts using the REST API. This is an alternative approach that fetches
    /// the full content items with all fields and linked content.
    /// </summary>
    /// <param name="locale">The locale to fetch posts for (e.g., "en-us")</param>
    /// <param name="take">Number of posts to fetch</param>
    /// <param name="contentLinkDepth">Depth for resolving linked content (e.g., Category)</param>
    /// <returns>List of PostDisplayItem objects</returns>
    Task<List<PostDisplayItem>> GetPostsRestAsync(string locale, int take = 10, int contentLinkDepth = 2);
}
