# Agility CMS & .NET Starter

To start using the Agility CMS & .NET Starter, [sign up](https://agilitycms.com/free) for a FREE account and create a new Instance using the DotNet Starter.

[Introduction to .NET and Agility CMS](https://agilitycms.com/docs/dotNet)

## About This Starter

- Uses the latest version of .NET, with greatly improved performance across many components, Language improvements to C# and F#, and much more.
- Supports full [Layout Management](https://agilitycms.com/docs/overview/layout-management)
- Supports Preview Mode

### Tailwind CSS

This starter uses [Tailwind CSS](https://tailwindcss.com/), a simple and lightweight utility-first CSS framework packed with classes that can be composed to build any design, directly in your markup.

It also comes equipped with [Autoprefixer](https://www.npmjs.com/package/autoprefixer), a plugin which use the data based on current browser popularity and property support to apply CSS prefixes for you.

This project is using Node.js to generate the tailwind css classes.

## Getting Started

🚨 Before you dive into the code, it's important that you have the latest version of the [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine (>=v8.0), as the project will _not_ run without this.

### Setting up the Starter

1. cd into the `Agility.NET.Starter` directory.
2. Rename the `appsetting.json.example` file to `appsettings.json`.
3. Overwrite the values in the `appsettings.json` file with the credentials from the API Keys page in [Agility Settings](https://app.agilitycms.com/settings/apikeys).

## Running the Site Locally

- `dotnet build` => Builds the website
- `dotnet run` => Builds & runs the website
- `npm run dev & dotnet watch` => Builds and runs the site in Watch mode, so changes are reflected in the browser immediately.
- `dotnet clean` => Cleans the build outputs of the site

## How It Works

- [How Pages/Layouts Work](https://agilitycms.com/docs/dotNet/how-pages-work-in-net)
- [How Components Work](https://agilitycms.com/docs/dotNet/how-page-modules-work-in-net)
- [How Layout Models/Templates Work](https://agilitycms.com/docs/dotNet/how-layout-models-work-in-net)

## Deploy to Azure App Service

1. Create an Web App (server) to host your application by clicking `Azure Deploy` button below: <br/>
   [![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fagility%2Fagilitycms-dotnet-starter%2Fmain%2FAgility.NET.Starter%2Fazuredeploy.json)

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

If you have feedback or questions about this starter, please use the [Github Issues](https://github.com/agility/agilitycms-dotnet-starter/issues) on this repo, join our [Community Slack Channel](https://agilitycms.com/join-slack).
