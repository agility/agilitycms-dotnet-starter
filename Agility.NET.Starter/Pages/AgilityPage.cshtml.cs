using System.Collections.Generic;
using System.Threading.Tasks;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Services;
using Agility.NET.Starter.Util.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Agility.NET.Starter.Pages
{
	public class AgilityPageModel : PageModel
	{
		private readonly FetchApiService _fetchApiService;

		public PageResponse PageResponse { get; set; }
		public List<ContentZone> ContentZones { get; set; }
		public string Locale { get; set; }
		public SitemapPage SitemapPage { get; set; }

		public AgilityPageModel(ILogger<AgilityPageModel> logger, IOptions<AppSettings> appSettings, FetchApiService fetchApiService)
		{
			_fetchApiService = fetchApiService;
		}

		public async Task<IActionResult> OnGetAsync()
		{
			var sitemapPage = (SitemapPage)RouteData.Values["agilityPage"];
			var isPreview = Util.Helpers.PreviewHelpers.IsPreviewMode(HttpContext);

			if (sitemapPage == null) return Page();

			var getPageExpandedParameters = new GetPageParameters()
			{
				PageId = sitemapPage.PageID,
				Locale = sitemapPage.Locale,
				ExpandAllContentLinks = true,
				ContentLinkDepth = 0,
				IsPreview = isPreview
			};

			var page = await _fetchApiService.GetTypedPage(getPageExpandedParameters);

			if (page == null) return Page();

			page.IsDynamicPage = (page.Dynamic != null);

			if (page.IsDynamicPage)
			{
				page.Name = page.IsDynamicPage ? sitemapPage.Name : page.Name;
				RouteData.Values["dynamicFields"] = page.Dynamic;

				var getItemParameters = new GetItemParameters()
				{
					ContentId = sitemapPage.ContentID,
					Locale = sitemapPage.Locale,
					ContentLinkDepth = 3,
					ExpandAllContentLinks = true,
					IsPreview = isPreview
				};



				var contentItem = await _fetchApiService.GetContentItem(getItemParameters);
				RouteData.Values["contentItem"] = contentItem;
			}


			PageResponse = page;
			Locale = sitemapPage.Locale;
			ContentZones = page.Zones;
			SitemapPage = sitemapPage;


			//setup the caching for CDN
			Response.Headers["Vary"] = "Agility-Mode";

			if (!isPreview)
			{
				//Set the cache control headers - this is just an example, you can set this to whatever you need
				//we are caching the page for 60 seconds, and allowing it to be served stale for up to 1 day seconds while we revalidate it
				Response.Headers["Cache-Control"] = "public, s-maxage=60, stale-while-revalidate=86400";
				Response.Headers["Agility-Mode"] = "production";
			}
			else
			{
				Response.Headers["Cache-Control"] = "no-store";
				Response.Headers["Agility-Mode"] = "preview";
			}
			return Page();

		}
	}
}
