using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.Blazor.Starter.Services.Cache;

namespace Agility.NET.Blazor.Starter.Services.Agility;

public partial class AgilityService
{
    public async Task<List<SitemapPage>> GetSitemapPagesAsync()
    {
        // Skip cache in preview mode
        if (IsPreviewMode)
        {
            return await FetchSitemapPagesAsync(isPreview: true);
        }

        // Use tag-based caching for all locales combined
        var locales = _appSettings.Locales.Split(",").Select(l => l.Trim()).ToList();

        // Create a combined cache tag for all sitemaps
        var cacheTag = string.Join("-", locales.Select(l => AgilityCacheKeys.SitemapFlat(l)));

        return await _cacheService.GetOrCreateAsync(
            cacheTag,
            async () => await FetchSitemapPagesAsync(isPreview: false)
        );
    }

    private async Task<List<SitemapPage>> FetchSitemapPagesAsync(bool isPreview)
    {
        var sitemapPages = new List<SitemapPage>();
        var locales = _appSettings.Locales.Split(",");

        foreach (var locale in locales)
        {
            var result = await _fetchApiService.GetSitemapFlat(new GetSitemapParameters
            {
                Locale = locale.Trim(),
                ChannelName = _appSettings.ChannelName,
                IsPreview = isPreview
            });

            var deserializedResult = DynamicHelpers.DeserializeSitemapFlat(result);
            deserializedResult.ForEach(s => s.Locale = locale.Trim());
            sitemapPages.AddRange(deserializedResult);
        }

        return sitemapPages;
    }

    public async Task<SitemapPage?> GetPageByPathAsync(string path)
    {
        var sitemapPages = await GetSitemapPagesAsync();

        if (string.IsNullOrEmpty(path))
        {
            return sitemapPages.FirstOrDefault();
        }

        return sitemapPages.Find(s => DoesPageMatchPath(s, path));
    }

    private static bool DoesPageMatchPath(SitemapPage sitemapPage, string path)
    {
        if (sitemapPage.IsFolder)
            return false;

        if (sitemapPage.Path == $"/{path}")
            return true;

        return $"/{sitemapPage.Locale}{sitemapPage.Path}" == $"/{path}";
    }

    public SitemapPage? CheckLocaleWithDifferentPageName(string path, List<SitemapPage> sitemapPages)
    {
        if (string.IsNullOrEmpty(path)) return null;

        var checkPath = path;
        var index = checkPath?.IndexOf("/", StringComparison.Ordinal);

        if (index < 0) return null;

        var locale = checkPath?.Substring(0, index ?? 0);

        if (string.IsNullOrEmpty(locale)) return null;

        var pageWithoutLocale = checkPath?.Replace(locale, string.Empty);
        var page = sitemapPages.FirstOrDefault(p => p.Path == pageWithoutLocale);

        var pageInDifferentLocaleAndName =
            sitemapPages.FirstOrDefault(s => s.PageID == page?.PageID && s.Locale == locale);

        return pageInDifferentLocaleAndName;
    }
}
