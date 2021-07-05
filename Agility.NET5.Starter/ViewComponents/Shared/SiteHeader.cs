using System.Linq;
using System.Threading.Tasks;
using Agility.NET5.FetchAPI.Helpers;
using Agility.NET5.FetchAPI.Models.API;
using Agility.NET5.FetchAPI.Models.Data;
using Agility.NET5.FetchAPI.Services;
using Agility.NET5.Starter.Models.ViewComponents;
using Agility.NET5.Starter.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Agility.NET5.Starter.ViewComponents.Shared
{
    public class SiteHeader : ViewComponent
    {
        private readonly FetchApiService _fetchApiService;
        private readonly AppSettings _appSettings;


        public SiteHeader(FetchApiService fetchApiService, IOptions<AppSettings> appSettings)
        {
            _fetchApiService = fetchApiService;
            _appSettings = appSettings.Value;
        }

        public async Task<IViewComponentResult> InvokeAsync(string locale)
        {
            var getItemParameters = new GetItemParameters()
            {
                ContentId = Constants.SiteHeaderContentId,
                Locale = locale
            };

            var content = await _fetchApiService.GetContentItem(getItemParameters);
            var siteHeader = DynamicHelpers.DeserializeTo<ContentItemResponse<Agility.Models.SiteHeader>>(content);

            var getSitemapParameters = new GetSitemapParameters()
            {
                ChannelName = _appSettings.ChannelName,
                Locale = locale
            };

            var sitemap = await _fetchApiService.GetSitemapFlat(getSitemapParameters);
            var deserializedSitemap = DynamicHelpers.DeserializeSitemapFlat(sitemap);
            deserializedSitemap = deserializedSitemap.Where(s => !string.IsNullOrEmpty(s.Name) && s.Visible.Menu).ToList();

            var siteHeaderModel = new SiteHeaderModel()
            {
                SiteHeader = siteHeader.Fields,
                SitemapPages = deserializedSitemap
            };

            return await Task.Run<IViewComponentResult>(() => View("/Views/Shared/SiteHeader.cshtml", siteHeaderModel));
        }
    }
}
