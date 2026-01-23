# Agility CMS & .NET Starter

This monorepo contains two .NET starter projects for Agility CMS:

| Project | Description | Framework |
|---------|-------------|-----------|
| [Agility.NET.MVC.Starter](./Agility.NET.MVC.Starter/) | Razor Pages implementation | ASP.NET Core Razor Pages |
| [Agility.NET.Blazor.Starter](./Agility.NET.Blazor.Starter/) | Blazor SSR implementation | Blazor Server-Side Rendering |

To start using these starters, [sign up](https://agilitycms.com/free) for a FREE account and create a new Instance using the DotNet Starter.

[Introduction to .NET and Agility CMS](https://agilitycms.com/docs/dotNet)

## About These Starters

Both starters share the same features:

- Uses the latest version of .NET 10, with greatly improved performance across many components
- Supports full [Layout Management](https://agilitycms.com/docs/overview/layout-management)
- Supports Preview Mode
- Uses Tailwind CSS v4 for styling
- Integrates with Agility CMS via the [Agility.NET.FetchAPI](https://www.nuget.org/packages/Agility.NET.FetchAPI) package

### Choose Your Framework

- **Razor Pages** (`Agility.NET.MVC.Starter`): Traditional server-rendered pages with the familiar Razor syntax. Great for SEO and simpler applications.

- **Blazor SSR** (`Agility.NET.Blazor.Starter`): Server-side rendered Blazor components. Enables component-based architecture while maintaining server-side rendering benefits.

## Prerequisites

Before you dive into the code, ensure you have:

- [.NET SDK](https://dotnet.microsoft.com/download) (>= v10.0)
- [Node.js](https://nodejs.org/) (for Tailwind CSS compilation)

## Getting Started

See the README in each project folder for specific setup instructions:

- [Razor Pages Starter README](./Agility.NET.MVC.Starter/README.md)
- [Blazor SSR Starter README](./Agility.NET.Blazor.Starter/README.md)

## Agility CMS Terminology

This starter uses Agility CMS's current terminology:

| Term | Description |
|------|-------------|
| **Components** | Reusable content modules that can be added to page zones (formerly called "Page Modules") |
| **Page Models** | Templates that define the structure and zones of a page (formerly called "Page Templates") |
| **Content Models** | Schemas that define the structure of content items |

## How It Works

- [How Pages/Layouts Work](https://agilitycms.com/docs/dotNet/how-pages-work-in-net)
- [How Components Work](https://agilitycms.com/docs/dotNet/how-page-modules-work-in-net)
- [How Page Models Work](https://agilitycms.com/docs/dotNet/how-layout-models-work-in-net)

## Deploy to Azure App Service

1. Create a Web App (server) to host your application by clicking the `Azure Deploy` button below: <br/>
   [![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fagility%2Fagilitycms-dotnet-starter%2Fmain%2FAgility.NET.MVC.Starter%2Fazuredeploy.json)

2. Deploy your source to Web App by following the steps here [How to Deploy the Dotnet Starter to Azure](https://agilitycms.com/docs/dotNet/deploy-net-site-to-azure)

## Resources

### Agility CMS

- [Official site](https://agilitycms.com)
- [Documentation](https://agilitycms.com/docs)

### .NET

- [Official site](https://dotnet.microsoft.com/)
- [Documentation](https://docs.microsoft.com/en-us/dotnet/)

### Tailwind CSS

- [Official site](http://tailwindcss.com/)
- [Documentation](http://tailwindcss.com/docs)

### Community

- [Official Slack](https://agilitycms.com/join-slack)
- [Blog](https://agilitycms.com/resources/posts)
- [GitHub](https://github.com/agility)
- [Facebook](https://www.facebook.com/AgilityCMS/)
- [X](https://x.com/AgilityCMS)

## Feedback and Questions

If you have feedback or questions about this starter, please use the [Github Issues](https://github.com/agility/agilitycms-dotnet-starter/issues) on this repo, or join our [Community Slack Channel](https://agilitycms.com/join-slack).
