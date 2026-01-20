using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;
using Agility.Net.Blazor.Starter.Services.Cache;

namespace Agility.Net.Blazor.Starter.Services.Agility;

public partial class AgilityService
{
    public async Task<PageResponse?> GetPageAsync(int pageId, string locale)
    {
        // Skip cache in preview mode
        if (IsPreviewMode)
        {
            return await FetchPageAsync(pageId, locale, isPreview: true);
        }

        var cacheTag = AgilityCacheKeys.Page(pageId, locale);

        return await _cacheService.GetOrCreateAsync(
            cacheTag,
            async () => await FetchPageAsync(pageId, locale, isPreview: false)
        );
    }

    private async Task<PageResponse?> FetchPageAsync(int pageId, string locale, bool isPreview)
    {
        var parameters = new GetPageParameters
        {
            PageId = pageId,
            Locale = locale,
            ExpandAllContentLinks = true,
            ContentLinkDepth = 0,
            IsPreview = isPreview
        };

        return await _fetchApiService.GetTypedPage(parameters);
    }

    public async Task<ContentItemResponse<T>?> GetContentItemAsync<T>(int contentId, string locale, int contentLinkDepth = 0) where T : class
    {
        // Skip cache in preview mode
        if (IsPreviewMode)
        {
            return await FetchContentItemAsync<T>(contentId, locale, contentLinkDepth, isPreview: true);
        }

        var cacheTag = AgilityCacheKeys.ContentItem(contentId, locale);

        return await _cacheService.GetOrCreateAsync(
            cacheTag,
            async () => await FetchContentItemAsync<T>(contentId, locale, contentLinkDepth, isPreview: false)
        );
    }

    private async Task<ContentItemResponse<T>?> FetchContentItemAsync<T>(int contentId, string locale, int contentLinkDepth, bool isPreview) where T : class
    {
        var parameters = new GetItemParameters
        {
            ContentId = contentId,
            Locale = locale,
            ContentLinkDepth = contentLinkDepth,
            ExpandAllContentLinks = true,
            IsPreview = isPreview
        };

        return await _fetchApiService.GetTypedContentItem<T>(parameters);
    }

    public async Task<List<T>> GetContentListAsync<T>(string referenceName, string locale, int take = 50, int contentLinkDepth = 0) where T : class
    {
        // Skip cache in preview mode
        if (IsPreviewMode)
        {
            return await FetchContentListAsync<T>(referenceName, locale, take, contentLinkDepth, isPreview: true);
        }

        var cacheTag = AgilityCacheKeys.ContentList(referenceName, locale);

        return await _cacheService.GetOrCreateAsync(
            cacheTag,
            async () => await FetchContentListAsync<T>(referenceName, locale, take, contentLinkDepth, isPreview: false)
        );
    }

    private async Task<List<T>> FetchContentListAsync<T>(string referenceName, string locale, int take, int contentLinkDepth, bool isPreview) where T : class
    {
        var parameters = new GetListParameters
        {
            ReferenceName = referenceName,
            Locale = locale,
            Take = take,
            ContentLinkDepth = contentLinkDepth,
            IsPreview = isPreview
        };

        var result = await _fetchApiService.GetTypedContentList<T>(parameters);
        return result.Items.Select(i => i.Fields).ToList();
    }

    public async Task<List<ContentItemResponse<T>>> GetContentByGraphQLAsync<T>(string query, string objName, string locale) where T : class
    {
        // Skip cache in preview mode - GraphQL queries are not cached by default
        // as they may have varying parameters. Use specific methods like GetPostsGraphQLAsync
        // for cacheable GraphQL queries.
        var result = await _fetchApiService.GetContentByGraphQL<T>(
            query: query,
            objName: objName,
            locale: locale,
            isPreview: IsPreviewMode
        );

        return result.ToList();
    }

    public async Task<UrlRedirectionsResponse?> GetUrlRedirectsAsync()
    {
        // Skip cache in preview mode
        if (IsPreviewMode)
        {
            return await FetchUrlRedirectsAsync();
        }

        var cacheTag = AgilityCacheKeys.UrlRedirections;

        return await _cacheService.GetOrCreateAsync(
            cacheTag,
            FetchUrlRedirectsAsync
        );
    }

    private async Task<UrlRedirectionsResponse?> FetchUrlRedirectsAsync()
    {
        var result = await _fetchApiService.GetUrlRedirections(new GetUrlRedirectionsParameters
        {
            LastAccessDate = DateTime.Now
        });

        return DynamicHelpers.DeserializeTo<UrlRedirectionsResponse>(result);
    }
}
