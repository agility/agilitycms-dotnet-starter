using Agility.NET.FetchAPI.Models.API;
using Agility.Net.Blazor.Starter.Services.Agility;

namespace Agility.Net.Blazor.Starter.Middleware;

public class AgilityRedirectMiddleware
{
    private readonly RequestDelegate _next;

    public AgilityRedirectMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAgilityService agilityService)
    {
        var path = context.Request.Path.Value;

        if (string.IsNullOrEmpty(path) || path == "/")
        {
            await _next(context);
            return;
        }

        // Skip static files
        if (IsStaticFile(path))
        {
            await _next(context);
            return;
        }

        var cleanPath = path.TrimStart('/');

        // Get sitemap pages and URL redirects
        var sitemapPages = await agilityService.GetSitemapPagesAsync();
        var urlRedirects = await agilityService.GetUrlRedirectsAsync();

        // Check URL redirects first
        if (CheckUrlRedirects(context, urlRedirects, cleanPath))
            return;

        // Check if we need to redirect to a different locale path
        if (CheckIfDifferentPageNameInLocale(context, agilityService, sitemapPages, cleanPath))
            return;

        await _next(context);
    }

    private static bool IsStaticFile(string path)
    {
        var staticExtensions = new[] { ".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".svg", ".ico", ".woff", ".woff2", ".ttf", ".eot", ".map" };
        return staticExtensions.Any(ext => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    private static bool CheckUrlRedirects(HttpContext context, UrlRedirectionsResponse? urlRedirects, string path)
    {
        try
        {
            if (urlRedirects?.Items == null || !urlRedirects.Items.Any())
                return false;

            foreach (var urlRedirect in urlRedirects.Items)
            {
                var originUrlCleaned = urlRedirect.OriginUrl.StartsWith('~')
                    ? urlRedirect.OriginUrl.TrimStart('~').TrimStart('/')
                    : urlRedirect.OriginUrl;

                if (path != originUrlCleaned)
                    continue;

                var destinationUrlCleaned = urlRedirect.DestinationUrl.StartsWith('~')
                    ? urlRedirect.DestinationUrl.TrimStart('~').TrimStart('/')
                    : urlRedirect.DestinationUrl;

                context.Response.Redirect(destinationUrlCleaned, permanent: true);
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    private static bool CheckIfDifferentPageNameInLocale(
        HttpContext context,
        IAgilityService agilityService,
        List<SitemapPage> sitemapPages,
        string path)
    {
        // Check if the page exists at this path
        var agilityPage = sitemapPages.Find(s => DoesAgilityPageExist(s, path));

        if (agilityPage != null)
            return false;

        // Check if this is a locale path that needs to redirect to a different page name
        agilityPage = agilityService.CheckLocaleWithDifferentPageName(path, sitemapPages);

        if (agilityPage == null)
            return false;

        context.Response.Redirect(
            $"{context.Request.Scheme}://{context.Request.Host}/{agilityPage.Locale}{agilityPage.Path}",
            permanent: true);

        return true;
    }

    private static bool DoesAgilityPageExist(SitemapPage sitemapPage, string path)
    {
        if (sitemapPage.IsFolder)
            return false;

        if (sitemapPage.Path == $"/{path}")
            return true;

        return $"/{sitemapPage.Locale}{sitemapPage.Path}" == $"/{path}";
    }
}

public static class AgilityRedirectMiddlewareExtensions
{
    public static IApplicationBuilder UseAgilityRedirects(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AgilityRedirectMiddleware>();
    }
}
