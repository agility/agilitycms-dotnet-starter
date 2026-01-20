# Agility CMS & Razor Pages Starter

This is an ASP.NET Core Razor Pages starter for Agility CMS, built with .NET 8.

## Features

- **Razor Pages**: Traditional server-rendered pages with the familiar Razor syntax
- **Agility CMS Integration**: Uses the [Agility.NET.FetchAPI](https://www.nuget.org/packages/Agility.NET.FetchAPI) package
- **Tailwind CSS**: Utility-first CSS framework with PostCSS and Autoprefixer
- **Dynamic Page Routing**: Automatic page routing based on Agility sitemap
- **View Components**: Reusable Agility components using ASP.NET View Components

## Project Structure

```
Agility.NET.Starter/
├── Helpers/                   # Helper classes
├── Middleware/                # Custom middleware
├── Models/                    # Content and view models
├── Pages/                     # Razor Pages
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   └── Index.cshtml           # Dynamic page handler
├── ViewComponents/
│   ├── PageModules/           # Agility CMS components
│   │   ├── FeaturedPost.cs
│   │   ├── Heading.cs
│   │   ├── PostDetails.cs
│   │   ├── PostsListing.cs
│   │   ├── RichTextArea.cs
│   │   └── TextBlockWithImage.cs
│   └── Shared/                # Shared view components
│       ├── SiteFooter.cs
│       └── SiteHeader.cs
├── Views/
│   ├── Shared/                # Shared views and layouts
│   │   ├── _Layout.cshtml
│   │   ├── _HeadScripts.cshtml
│   │   └── Components/        # View component templates
│   │       └── PageModules/
├── wwwroot/
│   └── css/
│       ├── input.css          # Tailwind input
│       └── output.css         # Generated CSS
├── Program.cs
├── Startup.cs
└── tailwind.config.js
```

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (>= v8.0)
- [Node.js](https://nodejs.org/) (for Tailwind CSS)

### Configuration

1. Copy `appsettings.json.example` to `appsettings.json`
2. Update the values with your Agility CMS credentials from [Agility Settings > API Keys](https://app.agilitycms.com/settings/apikeys):

```json
{
  "Agility": {
    "Guid": "your-instance-guid",
    "FetchAPIKey": "your-fetch-api-key",
    "PreviewAPIKey": "your-preview-api-key",
    "Locale": "en-us",
    "Channel": "website",
    "IsPreview": false
  }
}
```

### Install Dependencies

```bash
# Install .NET dependencies
dotnet restore

# Install Node dependencies (for Tailwind CSS)
npm install
```

### Running the Site

```bash
# Build and run
dotnet run

# Or run with hot reload
npm run dev & dotnet watch
```

The site will be available at `https://localhost:5001` (or the port specified in your launch settings).

## Adding New Components

To add a new Agility component:

1. **Create the View Component** in `ViewComponents/PageModules/MyNewComponent.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using Agility.NET.FetchAPI.Models;

namespace Agility.NET.Starter.ViewComponents.PageModules
{
    public class MyNewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(Module module, string locale)
        {
            // Fetch and transform your content
            return View(module);
        }
    }
}
```

2. **Create the View** in `Views/Shared/Components/PageModules/MyNewComponent/Default.cshtml`:

```razor
@model Agility.NET.FetchAPI.Models.Module

<div class="my-component">
    <h2>@Model.Item.Fields["title"]</h2>
    <p>@Model.Item.Fields["content"]</p>
</div>
```

3. The component will be automatically resolved based on the module name in Agility CMS.

## Agility CMS Terminology

| Term | Description |
|------|-------------|
| **Components** | Reusable content modules added to page zones (called "Page Modules" in this project's code) |
| **Page Models** | Templates defining page structure and zones |
| **Content Models** | Schemas defining content item structure |

> **Note**: This Razor Pages starter uses the older "PageModules" folder naming convention. See the Blazor starter for updated terminology.

## Tailwind CSS

This project uses Tailwind CSS with PostCSS. The CSS is compiled during build:

- **Input**: `wwwroot/css/input.css`
- **Output**: `wwwroot/css/output.css`
- **Config**: `tailwind.config.js`

To rebuild CSS manually:

```bash
npx tailwindcss -i ./wwwroot/css/input.css -o ./wwwroot/css/output.css
```

## Deploy to Azure

Click the button below to deploy to Azure App Service:

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fagility%2Fagilitycms-dotnet-starter%2Fmain%2FAgility.NET.Starter%2Fazuredeploy.json)

See [How to Deploy the Dotnet Starter to Azure](https://agilitycms.com/docs/dotNet/deploy-net-site-to-azure) for detailed instructions.

## Resources

- [Agility CMS Documentation](https://agilitycms.com/docs)
- [ASP.NET Core Razor Pages Documentation](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/)
- [Tailwind CSS Documentation](https://tailwindcss.com/docs)
