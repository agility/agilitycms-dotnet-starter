using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Services;
using Agility.NET.Blazor.Starter.Components;
using Agility.NET.Blazor.Starter.Endpoints;
using Agility.NET.Blazor.Starter.Middleware;
using Agility.NET.Blazor.Starter.Services.Agility;
using Agility.NET.Blazor.Starter.Services.Cache;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// Load appsettings.local.json for local development secrets (not committed to git)
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

// Add services to the container.
// AddInteractiveServerComponents enables SignalR-based interactivity for components that opt-in
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Persist data protection keys to survive dotnet watch restarts
// This prevents 403 errors from stale antiforgery cookies
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "keys")))
    .SetApplicationName("Agility.NET.Blazor.Starter");

// Configure Agility CMS settings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Configure cache control options for CDN (with defaults)
builder.Services.Configure<CacheControlOptions>(builder.Configuration.GetSection("CacheControl"));

// Add HTTP context accessor for preview mode detection
builder.Services.AddHttpContextAccessor();

// Add memory cache for sitemap and content caching
builder.Services.AddMemoryCache();

// Add Agility cache service for tag-based caching and invalidation
builder.Services.AddSingleton<IAgilityCacheService, AgilityCacheService>();

// Add HTTP client for Agility Fetch API
builder.Services.AddHttpClient<FetchApiService>();

// Add Agility service
builder.Services.AddScoped<IAgilityService, AgilityService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Static files MUST be served before routing kicks in
// This is critical for Blazor SSR with catch-all routes
app.UseStaticFiles();

app.UseRouting();

// Agility CMS redirects middleware - handles URL redirects and locale-based redirects
app.UseAgilityRedirects();

// Add stale-while-revalidate cache headers for CDN caching
// This must come before antiforgery to set headers on responses
app.UseCacheControl();

app.UseAntiforgery();

// Map Blazor components
// AddInteractiveServerRenderMode enables components to use @rendermode InteractiveServer
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map cache revalidation endpoint for Agility CMS webhooks
app.MapRevalidateEndpoint();

app.Run();
