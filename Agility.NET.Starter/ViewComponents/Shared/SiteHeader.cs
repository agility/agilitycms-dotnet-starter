using System.Linq;
using System.Threading.Tasks;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Services;
using Agility.NET.Starter.Models.ViewComponents;
using Agility.NET.Starter.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Agility.NET.Starter.ViewComponents.Shared
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

            var siteHeader = await _fetchApiService.GetTypedContentItem<Agility.Models.SiteHeader>(getItemParameters);

            var getSitemapParameters = new GetSitemapParameters()
            {
                ChannelName = _appSettings.ChannelName,
                Locale = locale
            };

            var sitemap = await _fetchApiService.GetTypedSitemapFlat(getSitemapParameters);
            sitemap = sitemap.Where(s => !string.IsNullOrEmpty(s.Name) && s.Visible.Menu).ToList();

            var siteHeaderModel = new SiteHeaderModel()
            {
                SiteHeader = siteHeader.Fields,
                SitemapPages = sitemap
            };

            return await Task.Run<IViewComponentResult>(() => View("/Views/Shared/SiteHeader.cshtml", siteHeaderModel));
        }
    }
}
