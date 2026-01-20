using Agility.NET.FetchAPI.Helpers;
using Microsoft.Extensions.Options;

namespace Agility.NET.Blazor.Starter.Middleware;

/// <summary>
/// Configuration options for CDN cache control headers.
/// </summary>
public class CacheControlOptions
{
    /// <summary>
    /// Maximum time (in seconds) that CDN serves cached content before revalidating.
    /// Default: 60 seconds (1 minute)
    /// </summary>
    public int MaxAgeSeconds { get; set; } = 60;

    /// <summary>
    /// Time (in seconds) that CDN can serve stale content while fetching fresh content in the background.
    /// Default: 300 seconds (5 minutes)
    /// </summary>
    public int StaleWhileRevalidateSeconds { get; set; } = 300;

    /// <summary>
    /// Time (in seconds) that CDN can serve stale content if the origin server is down.
    /// Default: 3600 seconds (1 hour)
    /// </summary>
    public int StaleIfErrorSeconds { get; set; } = 3600;
}

/// <summary>
/// Middleware that adds stale-while-revalidate cache control headers for CDN caching.
/// This allows pages to be served from CDN cache while revalidating in the background.
/// </summary>
public class CacheControlMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly CacheControlOptions _options;

    public CacheControlMiddleware(
        RequestDelegate next,
        IWebHostEnvironment env,
        IOptions<CacheControlOptions> options)
    {
        _next = next;
        _env = env;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Register callback to set headers before response starts
        context.Response.OnStarting(() =>
        {
            // Only add cache headers for successful HTML responses (pages)
            if (context.Response.StatusCode == 200 &&
                context.Response.ContentType?.Contains("text/html") == true)
            {
                // Check if this is a preview request
                var isPreview = context.Request.Query.ContainsKey("agilitypreviewkey") ||
                               context.Request.Cookies.ContainsKey("AgilityPreview_IsPreview");

                if (isPreview || _env.IsDevelopment())
                {
                    // No caching for preview mode or development
                    context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
                    context.Response.Headers["Pragma"] = "no-cache";
                }
                else
                {
                    // Production: stale-while-revalidate for CDN caching
                    var cacheControl = $"public, max-age={_options.MaxAgeSeconds}, " +
                                       $"stale-while-revalidate={_options.StaleWhileRevalidateSeconds}, " +
                                       $"stale-if-error={_options.StaleIfErrorSeconds}";
                    context.Response.Headers["Cache-Control"] = cacheControl;
                    context.Response.Headers["Vary"] = "Accept-Encoding";
                }
            }
            return Task.CompletedTask;
        });

        await _next(context);
    }
}

public static class CacheControlMiddlewareExtensions
{
    public static IApplicationBuilder UseCacheControl(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CacheControlMiddleware>();
    }
}
