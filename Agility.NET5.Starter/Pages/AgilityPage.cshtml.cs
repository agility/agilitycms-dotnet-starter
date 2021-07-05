using System.Collections.Generic;
using System.Threading.Tasks;
using Agility.NET5.FetchAPI.Helpers;
using Agility.NET5.FetchAPI.Models.API;
using Agility.NET5.FetchAPI.Models.Data;
using Agility.NET5.FetchAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Agility.NET5.Starter.Pages
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
            var sitemapPage = (SitemapPage) RouteData.Values["agilityPage"];

            if (sitemapPage == null) return Page();

            var getPageExpandedParameters = new GetPageParameters()
            {
                PageId = sitemapPage.PageID,
                Locale = sitemapPage.Locale,
                ExpandAllContentLinks = true,
                ContentLinkDepth = 2
            };

            var content = await _fetchApiService.GetPage(getPageExpandedParameters);
            var deserializedContent = DynamicHelpers.DeserializeTo<PageResponse>(content);

            if (deserializedContent == null) return Page();
            
            deserializedContent.IsDynamicPage = (deserializedContent.Dynamic != null);

            if (deserializedContent.IsDynamicPage)
            {
                deserializedContent.Name = deserializedContent.IsDynamicPage ? sitemapPage.Name : deserializedContent.Name;
                RouteData.Values["dynamicFields"] = deserializedContent.Dynamic;

                var getItemParameters = new GetItemParameters()
                {
                    ContentId = sitemapPage.ContentID,
                    Locale = sitemapPage.Locale,
                    ContentLinkDepth = 3,
                    ExpandAllContentLinks = true
                };

                var contentItem = await _fetchApiService.GetContentItem(getItemParameters);
                RouteData.Values["contentItem"] = contentItem;
            }

            List<ContentZone> contentZonesExpanded = DynamicHelpers.DeserializeContentZones(deserializedContent.Zones.ToString());

            PageResponse = deserializedContent;
            Locale = sitemapPage.Locale;
            ContentZones = contentZonesExpanded;
            SitemapPage = sitemapPage;

            return Page();

        }
    }
}
