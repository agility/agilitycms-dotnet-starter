using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Services;
using Agility.Net.Blazor.Starter.Services.Cache;
using Agility.Net.Blazor.Starter.Util;
using Microsoft.Extensions.Options;

namespace Agility.Net.Blazor.Starter.Services.Agility;

/// <summary>
/// Main service for interacting with Agility CMS.
/// This is a partial class - see the following files for specific functionality:
/// - AgilityService.Sitemap.cs: Sitemap and page routing methods
/// - AgilityService.Content.cs: Content retrieval methods (pages, items, lists, GraphQL)
/// - AgilityService.Posts.cs: Post-specific methods (GraphQL and REST examples)
/// </summary>
public partial class AgilityService : IAgilityService
{
    private readonly FetchApiService _fetchApiService;
    private readonly AppSettings _appSettings;
    private readonly IAgilityCacheService _cacheService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _env;

    public AgilityService(
        FetchApiService fetchApiService,
        IOptions<AppSettings> appSettings,
        IAgilityCacheService cacheService,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment env)
    {
        _fetchApiService = fetchApiService;
        _appSettings = appSettings.Value;
        _cacheService = cacheService;
        _httpContextAccessor = httpContextAccessor;
        _env = env;
    }

    public bool IsPreviewMode
    {
        get
        {
            if (_env.IsDevelopment())
                return true;

            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null) return false;

                // Check for preview key in query string
                var agilityPreviewKey = httpContext.Request.Query[Constants.AgilityPreviewKeyName].ToString();
                if (!string.IsNullOrEmpty(agilityPreviewKey) &&
                    agilityPreviewKey == PreviewHelpers.GenerateAgilityPreviewKey(_appSettings.SecurityKey))
                {
                    return true;
                }

                // Check for preview cookie
                var isPreviewCookie = httpContext.Request.Cookies[Constants.IsPreviewCookieName]?.ToLower();
                return isPreviewCookie == "true";
            }
            catch
            {
                // HttpContext may not be available in SignalR circuits
                return false;
            }
        }
    }

    public void SetPreviewMode(bool isPreview)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return;

        httpContext.Response.Cookies.Delete(Constants.IsPreviewCookieName);
        httpContext.Response.Cookies.Append(
            Constants.IsPreviewCookieName,
            isPreview ? "true" : "false",
            new CookieOptions { Path = "/" }
        );
    }
}
