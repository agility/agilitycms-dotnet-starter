# Agility CMS & Blazor SSR Starter

This is a Blazor Server-Side Rendering (SSR) starter for Agility CMS, built with .NET 10.

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fagility%2Fagilitycms-dotnet-starter%2Fmain%2FAgility.NET.Blazor.Starter%2Fazuredeploy.json)

## Features:

- **Blazor SSR**: Server-side rendered Blazor components for optimal performance and SEO
- **Interactive Server Components**: Opt-in interactivity using SignalR for dynamic features like infinite scroll
- **Agility CMS Integration**: Uses the [Agility.NET.FetchAPI](https://www.nuget.org/packages/Agility.NET.FetchAPI) package
- **GraphQL Support**: Query content using Agility's GraphQL API
- **Tailwind CSS v4**: Modern utility-first CSS framework with typography plugin
- **Dynamic Page Routing**: Automatic page routing based on Agility sitemap
- **Component-Based Architecture**: Reusable Agility components
- **CDN-Ready Caching**: Stale-while-revalidate cache headers for optimal CDN performance
- **URL Redirects**: Built-in middleware for Agility CMS URL redirections

## Architecture

This starter uses **Blazor Static SSR** as the default rendering mode, with the ability to opt specific components into **Interactive Server** mode when client-side interactivity is needed.

### Render Modes

| Mode                     | Description                                  | Use Case                           |
| ------------------------ | -------------------------------------------- | ---------------------------------- |
| **Static SSR** (default) | Server renders HTML, no client interactivity | Most pages/components              |
| **Interactive Server**   | SignalR connection for real-time updates     | Infinite scroll, forms, dynamic UI |

### Server/Client Component Pattern

For components requiring interactivity (like the Posts Listing with infinite scroll), we follow a pattern similar to Next.js:

1. **Server Component** - Fetches initial data during SSR
2. **Client Component** - Handles interactive features via SignalR

Example: `PostsListing/`

- `PostsListing.razor` - Server component, fetches initial posts
- `PostsListingClient.razor` - Interactive component with infinite scroll

## Project Structure

```
Agility.NET.Blazor.Starter/
├── Components/
│   ├── AgilityComponents/        # Agility CMS components (content modules)
│   │   ├── FeaturedPost.razor
│   │   ├── Heading.razor
│   │   ├── PostDetails.razor
│   │   ├── PostsListing/         # Server/Client component pattern
│   │   │   ├── PostsListing.razor       # Server component
│   │   │   └── PostsListingClient.razor # Interactive client component
│   │   ├── RichTextArea.razor
│   │   └── TextBlockWithImage.razor
│   ├── Layout/                   # Layout components
│   │   └── MainLayout.razor
│   ├── Pages/                    # Page components
│   │   └── AgilityPage.razor     # Dynamic page handler
│   └── Shared/                   # Shared components
│       ├── AgilityComponent.razor    # Dynamic component renderer
│       ├── SEO.razor
│       ├── SiteFooter.razor
│       └── SiteHeader.razor
├── Middleware/
│   ├── AgilityRedirectsMiddleware.cs  # URL redirects
│   └── CacheControlMiddleware.cs      # CDN cache headers
├── Models/
│   ├── AgilityModels.cs          # Content models
│   └── PostDisplayItem.cs        # Display models
├── Services/
│   ├── Agility/                 # Agility service layer
│   │   ├── IAgilityService.cs
│   │   ├── AgilityService.cs
│   │   ├── AgilityService.Content.cs
│   │   ├── AgilityService.Sitemap.cs
│   │   └── AgilityService.Posts.cs
│   └── Cache/                   # Cache service with tag-based invalidation
│       ├── AgilityCacheKeys.cs
│       ├── IAgilityCacheService.cs
│       └── AgilityCacheService.cs
├── Styles/
│   └── tailwind.css              # Tailwind v4 config
├── wwwroot/
│   └── css/
│       └── output.css            # Generated CSS
└── Program.cs
```

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (>= v10.0)
- [Node.js](https://nodejs.org/) (for Tailwind CSS)

### Configuration

Create `appsettings.local.json` with your Agility CMS credentials from [Agility Settings > API Keys](https://app.agilitycms.com/settings/apikeys):

```json
{
	"AppSettings": {
		"InstanceGUID": "your-instance-guid",
		"SecurityKey": "your-security-key",
		"FetchAPIKey": "your-fetch-api-key",
		"PreviewAPIKey": "your-preview-api-key"
	}
}
```

This file is **gitignored** and won't be committed. The base `appsettings.json` contains non-sensitive defaults.

> **Tip**: In development, `CacheInMinutes` is automatically set to `0` via `appsettings.Development.json` so you always see fresh content.

### Install Dependencies

```bash
# Install .NET dependencies
dotnet restore

# Install Node dependencies (for Tailwind CSS)
npm install
```

### Running the Site

```bash
# Run with hot reload (recommended for development)
dotnet watch

# Tailwind will rebuild automatically via the build target
```

The site will be available at `http://localhost:5000`.

## Configuration Reference

All settings can be configured via `appsettings.json` or environment variables. For Azure App Service, use the Environment Variables section in the Configuration blade.

### AppSettings

| Setting          | Default   | Env Variable                  | Description                                        |
| ---------------- | --------- | ----------------------------- | -------------------------------------------------- |
| `InstanceGUID`   | -         | `AppSettings__InstanceGUID`   | Your Agility CMS instance GUID                     |
| `SecurityKey`    | -         | `AppSettings__SecurityKey`    | Security key for preview mode validation           |
| `FetchAPIKey`    | -         | `AppSettings__FetchAPIKey`    | API key for fetching published content             |
| `PreviewAPIKey`  | -         | `AppSettings__PreviewAPIKey`  | API key for fetching preview/draft content         |
| `Locales`        | `en-us`   | `AppSettings__Locales`        | Comma-separated locale codes (e.g., `en-us,fr-ca`) |
| `ChannelName`    | `website` | `AppSettings__ChannelName`    | Agility CMS channel/sitemap name                   |
| `CacheInMinutes` | `5`       | `AppSettings__CacheInMinutes` | In-memory cache duration for API responses         |

> **Note**: `CacheInMinutes` controls server-side caching of Agility API responses. Set to `0` to disable (useful for development). In preview mode, caching is always disabled regardless of this setting.

### CacheControl (CDN Headers)

| Setting                       | Default | Env Variable                                | Description                                                         |
| ----------------------------- | ------- | ------------------------------------------- | ------------------------------------------------------------------- |
| `MaxAgeSeconds`               | `30`    | `CacheControl__MaxAgeSeconds`               | CDN serves cached content for this duration before revalidating     |
| `StaleWhileRevalidateSeconds` | `86400` | `CacheControl__StaleWhileRevalidateSeconds` | CDN serves stale content while fetching fresh in background (1 day) |
| `StaleIfErrorSeconds`         | `86400` | `CacheControl__StaleIfErrorSeconds`         | CDN serves stale content if origin is down (1 day)                  |

Cache headers are **disabled** for:

- Preview mode requests
- Development environment

### Azure App Service Environment Variables

When deploying to Azure App Service (Linux), configure settings using double underscores (`__`) for nested properties:

```
AppSettings__InstanceGUID=your-guid-here
AppSettings__SecurityKey=your-security-key
AppSettings__FetchAPIKey=defaultlive$...
AppSettings__PreviewAPIKey=defaultpreview$...
AppSettings__Locales=en-us
AppSettings__ChannelName=website
AppSettings__CacheInMinutes=5
CacheControl__MaxAgeSeconds=30
CacheControl__StaleWhileRevalidateSeconds=86400
CacheControl__StaleIfErrorSeconds=86400
```

You can set these in the Azure Portal under **Configuration > Application settings**, or via Azure CLI:

```bash
az webapp config appsettings set --name your-app-name --resource-group your-rg \
  --settings AppSettings__InstanceGUID=your-guid AppSettings__FetchAPIKey=your-key
```

## Adding Interactive Components

To add a new interactive component:

1. **Create the server component** (fetches data):

```razor
@* Components/AgilityComponents/MyFeature/MyFeature.razor *@
@inject IAgilityService AgilityService

@if (data != null)
{
    <MyFeatureClient InitialData="data" />
}

@code {
    [Parameter]
    public ModuleModel ComponentData { get; set; } = new();

    private MyData? data;

    protected override async Task OnInitializedAsync()
    {
        // Fetch data server-side
        data = await AgilityService.GetContentListAsync<MyData>(...);
    }
}
```

2. **Create the client component** (handles interactivity):

```razor
@* Components/AgilityComponents/MyFeature/MyFeatureClient.razor *@
@rendermode @(new InteractiveServerRenderMode(prerender: true))
@inject IJSRuntime JSRuntime

<div>
    @* Interactive UI here *@
</div>

@code {
    [Parameter]
    public MyData InitialData { get; set; }

    // Client-side logic via SignalR
}
```

3. **Add namespace** to `_Imports.razor`:

```razor
@using Agility.NET.Blazor.Starter.Components.AgilityComponents.MyFeature
```

4. **Register** in `AgilityComponent.razor`:

```razor
case "MyFeature":
    <MyFeature ComponentData="@Component" />
    break;
```

## Adding Static Components

For components that don't need interactivity:

1. **Create the component** in `Components/AgilityComponents/`:

```razor
@* MyComponent.razor *@
@inject IAgilityService AgilityService

@if (Content != null)
{
    <div class="my-component">
        <h2>@Content.Fields.Title</h2>
        @((MarkupString)Content.Fields.Content)
    </div>
}

@code {
    [Parameter]
    public ModuleModel ComponentData { get; set; } = new();

    private ContentItemResponse<MyModel>? Content { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (ComponentData.Model?.Item?.ContentID > 0)
        {
            Content = await AgilityService.GetContentItemAsync<MyModel>(
                ComponentData.Model.Item.ContentID,
                ComponentData.Locale
            );
        }
    }
}
```

2. **Register** in `AgilityComponent.razor`

## AgilityPic Component

The `AgilityPic` component provides responsive image support using the Agility Image API. It outputs a `<picture>` element with multiple `<source>` tags for different viewport sizes, similar to the Next.js SDK's AgilityPic component.

### Basic Usage

```razor
<AgilityPic Image="@Content.Fields.Image" FallbackWidth="800" />
```

### With Responsive Sources

Use the `AgilitySource` sub-component to define different image sizes for different viewport widths:

```razor
<AgilityPic Image="@Content.Fields.Image"
            Class="object-cover w-full h-full"
            FallbackWidth="800"
            Priority="true">
    <AgilitySource Media="(min-width: 1280px)" Width="1200" />
    <AgilitySource Media="(min-width: 640px)" Width="800" />
    <AgilitySource Media="(max-width: 639px)" Width="640" />
</AgilityPic>
```

### AgilityPic Parameters

| Parameter       | Type               | Default       | Description                                                   |
| --------------- | ------------------ | ------------- | ------------------------------------------------------------- |
| `Image`         | `ImageAttachment?` | required      | The Agility image attachment object                           |
| `FallbackWidth` | `int?`             | null          | Width for the fallback `<img>` tag (uses original if not set) |
| `Alt`           | `string?`          | `Image.Label` | Alt text override (defaults to image label from CMS)          |
| `Class`         | `string?`          | null          | CSS classes for the `<img>` element                           |
| `Priority`      | `bool`             | false         | If true, sets `loading="eager"` instead of `loading="lazy"`   |

### AgilitySource Parameters

| Parameter | Type     | Default  | Description                                    |
| --------- | -------- | -------- | ---------------------------------------------- |
| `Media`   | `string` | required | CSS media query (e.g., `"(min-width: 768px)"`) |
| `Width`   | `int`    | 0        | Target width for the image                     |
| `Height`  | `int`    | 0        | Target height for the image (optional)         |

### Generated Output

The component generates optimized image URLs using the Agility Image API with `?format=auto&w=X` parameters:

```html
<picture>
	<source media="(min-width: 1280px)" srcset="https://cdn.agilitycms.com/image.jpg?format=auto&w=1200" />
	<source media="(min-width: 640px)" srcset="https://cdn.agilitycms.com/image.jpg?format=auto&w=800" />
	<source media="(max-width: 639px)" srcset="https://cdn.agilitycms.com/image.jpg?format=auto&w=640" />
	<img
		src="https://cdn.agilitycms.com/image.jpg?format=auto&w=800"
		alt="Image label from CMS"
		loading="eager"
		class="object-cover w-full h-full"
	/>
</picture>
```

### Best Practices

- **Order sources from largest to smallest** - browsers use the first matching source
- **Use `Priority="true"` for above-the-fold images** - this disables lazy loading for LCP optimization
- **Set appropriate FallbackWidth** - this is used for browsers that don't support `<picture>`
- **Don't set Alt unless overriding** - the component defaults to the image's Label from the CMS

## Preview Mode

The starter supports Agility CMS preview mode:

- **Development**: Always in preview mode
- **Production**: Preview activated via `agilitypreviewkey` query parameter or cookie

Preview mode:

- Fetches draft/staging content
- Disables caching
- Shows preview indicator bar

## Tailwind CSS v4

This project uses Tailwind CSS v4 with the typography plugin. Configuration is in `Styles/tailwind.css`:

```css
@import "tailwindcss";
@plugin "@tailwindcss/typography";

@theme {
	--color-primary-500: #6415ff;
	--color-secondary-500: #243e63;
	/* ... */
}

@source "../Components/**/*.razor";
```

To rebuild CSS manually:

```bash
npx @tailwindcss/cli -i ./Styles/tailwind.css -o ./wwwroot/css/output.css --watch
```

## Agility CMS Terminology

| Term               | Description                                  |
| ------------------ | -------------------------------------------- |
| **Components**     | Reusable content modules added to page zones |
| **Page Models**    | Templates defining page structure and zones  |
| **Content Models** | Schemas defining content item structure      |
| **Containers**     | Lists or single items of content             |

## Troubleshooting

### 403 Errors After Server Restart

The starter persists data protection keys to survive `dotnet watch` restarts. If you still see 403 errors:

1. Clear browser cookies for localhost
2. Or hard refresh (Ctrl+Shift+R / Cmd+Shift+R)

### SignalR Not Connecting

Ensure Interactive Server mode is properly configured:

1. `Program.cs` has `.AddInteractiveServerComponents()` and `.AddInteractiveServerRenderMode()`
2. Component has `@rendermode @(new InteractiveServerRenderMode(prerender: true))`
3. Check browser Network tab for `/_blazor/negotiate` request

### Tailwind Classes Not Working

1. Ensure the `@source` directive in `tailwind.css` covers your component files
2. Run `npm run build:css` to rebuild
3. Check that `output.css` is being served

## Deploy to Azure

Click the button below to deploy this starter to Azure App Service:

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fagility%2Fagilitycms-dotnet-starter%2Fmain%2FAgility.NET.Blazor.Starter%2Fazuredeploy.json)

### Deployment Parameters

| Parameter       | Required | Default           | Description                                        |
| --------------- | -------- | ----------------- | -------------------------------------------------- |
| `webAppName`    | Yes      | agilitycms-blazor | Globally unique name for your app                  |
| `instanceGuid`  | Yes      | -                 | From Agility CMS Settings > API Keys               |
| `securityKey`   | Yes      | -                 | From Agility CMS Settings > API Keys               |
| `fetchAPIKey`   | Yes      | -                 | From Agility CMS Settings > API Keys               |
| `previewAPIKey` | Yes      | -                 | From Agility CMS Settings > API Keys               |
| `locales`       | No       | en-us             | Comma-separated locale codes                       |
| `channelName`   | No       | website           | Agility CMS channel name                           |
| `sku`           | No       | B1                | App Service Plan SKU (B1+ required for WebSockets) |

### What Gets Deployed

- **Linux App Service** running .NET 10
- **WebSockets enabled** (required for Blazor Interactive Server mode)
- **AlwaysOn** enabled for better performance
- **HTTP/2** enabled
- **Git deployment** from this repository

### Post-Deployment Setup

1. **Configure Webhooks** in Agility CMS:
   - Go to Settings > Webhooks
   - Add a new webhook pointing to `https://your-app.azurewebsites.net/api/revalidate`
   - This enables automatic cache invalidation when content is published

2. **Configure Preview** in Agility CMS:
   - Go to Settings > Preview URLs
   - Add your Azure URL with the preview query parameter

### SKU Recommendations

| SKU   | WebSockets | AlwaysOn | Recommended For         |
| ----- | ---------- | -------- | ----------------------- |
| B1    | ✅         | ✅       | Development/Testing     |
| S1+   | ✅         | ✅       | Production              |
| P1V2+ | ✅         | ✅       | High-traffic Production |

> **Note**: Free (F1) and Shared (D1) tiers do not support WebSockets or AlwaysOn, which are required for Blazor Interactive Server components.

## Cache Revalidation

This starter includes a webhook endpoint for cache invalidation at `/api/revalidate`. When content is published in Agility CMS, the webhook invalidates the relevant cached data:

- **Content changes**: Invalidates content list and item caches
- **Page changes**: Invalidates page and sitemap caches
- **URL Redirections**: Invalidates the URL redirections cache

### Cache Management Endpoints

| Endpoint           | Method | Description                                |
| ------------------ | ------ | ------------------------------------------ |
| `/api/revalidate`  | POST   | Webhook for Agility CMS cache invalidation |
| `/api/cache/tags`  | GET    | View all cached tags (debugging)           |
| `/api/cache/clear` | POST   | Clear all cached data                      |

## Resources

- [Agility CMS Documentation](https://agilitycms.com/docs)
- [Blazor Documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/)
- [Blazor Render Modes](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes)
- [Tailwind CSS v4 Documentation](https://tailwindcss.com/docs)
