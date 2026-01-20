using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace Agility.NET.Starter.Util.Helpers
{
    public static class TransformerMiddlewareHelpers
    {
        public static async Task<List<SitemapPage>> GetSitemapPages(AppSettings appSettings, FetchApiService fetchApiService, bool isPreview)
        {
            var sitemapPages = new List<SitemapPage>();
            var locales = appSettings.Locales.Split(",");

            foreach (var locale in locales)
            {

                var result = await fetchApiService.GetSitemapFlat(new GetSitemapParameters()
                {
                    Locale = locale,
                    ChannelName = appSettings.ChannelName,
                    IsPreview = isPreview
                });
                var deserializedResult = DynamicHelpers.DeserializeSitemapFlat(result);

                deserializedResult.ForEach(s => s.Locale = locale);

                sitemapPages.AddRange(deserializedResult);
            }

            return sitemapPages;
        }

        public static async Task<UrlRedirectionsResponse> GetUrlRedirects(FetchApiService fetchApiService)
        {

            var result = await fetchApiService.GetUrlRedirections(new GetUrlRedirectionsParameters()
            {
                LastAccessDate = DateTime.Now
            });
            return DynamicHelpers.DeserializeTo<UrlRedirectionsResponse>(result);
        }

        public static SitemapPage? CheckLocaleWithDifferentPageName(string? path, List<SitemapPage> sitemapPages)
        {
            if (string.IsNullOrEmpty(path)) return null;

            var checkPath = path;

            var index = checkPath.IndexOf("/", StringComparison.Ordinal);

            if (index < 0) return null;

            var locale = checkPath.Substring(0, index);

            if (string.IsNullOrEmpty(locale)) return null;

            var pageWithoutLocale = checkPath.Replace(locale, string.Empty);
            var page = sitemapPages.FirstOrDefault(p => p.Path == pageWithoutLocale);

            var pageInDifferentLocaleAndName =
                sitemapPages.FirstOrDefault(s => s.PageID == page?.PageID && s.Locale == locale);

            return pageInDifferentLocaleAndName;
        }

        public static async Task<List<SitemapPage>> GetOrCacheSitemapPages(
            IWebHostEnvironment env,
            AppSettings appSettings,
            FetchApiService fetchApiService,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache cache)
        {
            SetAgilityPreviewKeyIfValid(httpContextAccessor, appSettings);

            bool isPreview = PreviewHelpers.IsPreviewMode(httpContextAccessor);

            if (env.IsDevelopment())
            {
                //force preview mode in development
                PreviewHelpers.SetPreviewMode(httpContextAccessor, true);
                isPreview = true;

            }


            if (isPreview)
            {
                return await GetSitemapPages(appSettings, fetchApiService, isPreview: true);
            }

            var key = Constants.SitemapPagesKey;

            if (cache.TryGetValue(key, out List<SitemapPage>? cachedSitemapPages) && cachedSitemapPages != null) return cachedSitemapPages;

            var sitemapPages = await GetSitemapPages(appSettings, fetchApiService, isPreview: false);

            var cacheExpirationOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(appSettings.CacheInMinutes),
                Priority = CacheItemPriority.Normal
            };

            cache.Set(key, sitemapPages, cacheExpirationOptions);
            return sitemapPages;
        }

        public static async Task<UrlRedirectionsResponse> GetOrCacheUrlRedirects(
            IWebHostEnvironment env,
            AppSettings appSettings,
            FetchApiService fetchApiService,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache cache)
        {

            SetAgilityPreviewKeyIfValid(httpContextAccessor, appSettings);

            if (env.IsDevelopment())
            {
                return await GetUrlRedirects(fetchApiService);
            }

            bool isPreview = PreviewHelpers.IsPreviewMode(httpContextAccessor);

            if (isPreview)
            {
                return await GetUrlRedirects(fetchApiService);
            }

            var key = Constants.UrlRedirectionsResponseKey;

            if (cache.TryGetValue(key, out UrlRedirectionsResponse? cachedUrlRedirectionsResponse) && cachedUrlRedirectionsResponse != null) return cachedUrlRedirectionsResponse;

            var urlRedirectionsResponse = await GetUrlRedirects(fetchApiService);

            var cacheExpirationOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(appSettings.CacheInMinutes),
                Priority = CacheItemPriority.Normal
            };

            cache.Set(key, urlRedirectionsResponse, cacheExpirationOptions);
            return urlRedirectionsResponse;
        }

        public static void SetAgilityPreviewKeyIfValid(
            IHttpContextAccessor httpContextAccessor,
            AppSettings appSettings
            )
        {
            var agilityPreviewKey = httpContextAccessor.HttpContext?.Request.Query[Constants.AgilityPreviewKeyName]
                .ToString();

            if (!string.IsNullOrEmpty(agilityPreviewKey)
                && agilityPreviewKey == Agility.NET.FetchAPI.Helpers.PreviewHelpers.GenerateAgilityPreviewKey(appSettings.SecurityKey))
            {
                //we are kicking into preview mode
                // Set the preview cookie
                PreviewHelpers.SetPreviewCookie(httpContextAccessor);

                // Set the IsPreview flag in the HttpContext
                PreviewHelpers.SetPreviewMode(httpContextAccessor, true);
            }
            else if (PreviewHelpers.CheckPreviewCookie(httpContextAccessor))
            {
                //if the preview cookie is already set, we are in preview mode
                // Set the IsPreview flag in the HttpContext
                PreviewHelpers.SetPreviewMode(httpContextAccessor, true);
            }
        }

        public static bool DoesAgilityPageExist(SitemapPage sitemapPage, string path)
        {
            if (sitemapPage.IsFolder)
                return false;

            if (sitemapPage.Path == $@"/{path}")
                return true;

            return $@"/{sitemapPage.Locale}{sitemapPage.Path}" == $"/{path}";
        }

    }
}
