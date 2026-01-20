using Agility.Models;
using Agility.Net.Blazor.Starter.Models;
using Agility.Net.Blazor.Starter.Services.Cache;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;

namespace Agility.Net.Blazor.Starter.Services.Agility;

public partial class AgilityService
{
    /// <summary>
    /// Gets posts using the GraphQL API. This is the recommended approach for fetching posts
    /// as it allows for precise field selection and efficient data retrieval.
    /// </summary>
    public async Task<List<PostDisplayItem>> GetPostsGraphQLAsync(string locale, int take = 10, int skip = 0)
    {
        // Skip cache in preview mode
        if (IsPreviewMode)
        {
            return await FetchPostsGraphQLAsync(locale, take, skip, isPreview: true);
        }

        // Cache key includes take and skip for pagination support
        var cacheTag = $"{AgilityCacheKeys.GraphQL("posts", locale)}-{take}-{skip}";

        return await _cacheService.GetOrCreateAsync(
            cacheTag,
            async () => await FetchPostsGraphQLAsync(locale, take, skip, isPreview: false)
        );
    }

    private async Task<List<PostDisplayItem>> FetchPostsGraphQLAsync(string locale, int take, int skip, bool isPreview)
    {
        var posts = await _fetchApiService.GetContentByGraphQL<Post>(
            query: $@"
                query {{
                    posts (take: {take}, skip: {skip}, sort: ""fields.date"", direction: ""desc"") {{
                        contentID
                        fields {{
                            title
                            slug
                            date
                            image {{
                                label
                                url
                            }}
                            category {{
                                fields {{
                                    title
                                }}
                            }}
                        }}
                    }}
                }}",
            objName: "posts",
            locale: locale,
            isPreview: isPreview
        );

        return posts.Select(p => new PostDisplayItem
        {
            ContentID = p.ContentID,
            Title = p.Fields?.Title,
            CategoryTitle = p.Fields?.Category?.Fields?.Title,
            Slug = p.Fields?.Slug,
            DateDisplay = p.Fields?.Date.ToString("MMM. dd, yyyy"),
            ImageUrl = p.Fields?.Image?.Url,
            ImageLabel = p.Fields?.Image?.Label,
            IsPhantom = false
        }).ToList();
    }

    /// <summary>
    /// Gets posts using the REST API. This is an alternative approach that fetches
    /// the full content items with all fields and linked content.
    /// </summary>
    public async Task<List<PostDisplayItem>> GetPostsRestAsync(string locale, int take = 10, int contentLinkDepth = 2)
    {
        // Skip cache in preview mode
        if (IsPreviewMode)
        {
            return await FetchPostsRestAsync(locale, take, contentLinkDepth, isPreview: true);
        }

        var cacheTag = AgilityCacheKeys.ContentList("posts", locale);

        return await _cacheService.GetOrCreateAsync(
            cacheTag,
            async () => await FetchPostsRestAsync(locale, take, contentLinkDepth, isPreview: false)
        );
    }

    private async Task<List<PostDisplayItem>> FetchPostsRestAsync(string locale, int take, int contentLinkDepth, bool isPreview)
    {
        var parameters = new GetListParameters
        {
            ReferenceName = "posts",
            Locale = locale,
            Take = take,
            ContentLinkDepth = contentLinkDepth,
            IsPreview = isPreview
        };

        var result = await _fetchApiService.GetTypedContentList<Post>(parameters);

        return result.Items
            .OrderByDescending(p => p.Fields?.Date)
            .Select(p => new PostDisplayItem
            {
                ContentID = p.ContentID,
                Title = p.Fields?.Title,
                CategoryTitle = p.Fields?.Category?.Fields?.Title,
                Slug = p.Fields?.Slug,
                DateDisplay = p.Fields?.Date.ToString("MMM. dd, yyyy"),
                ImageUrl = p.Fields?.Image?.Url,
                ImageLabel = p.Fields?.Image?.Label,
                IsPhantom = false
            }).ToList();
    }
}
