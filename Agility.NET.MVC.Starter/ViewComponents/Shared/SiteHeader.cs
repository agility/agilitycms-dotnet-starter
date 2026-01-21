using System.Linq;
using System.Threading.Tasks;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Services;
using Agility.NET.MVC.Starter.Models.ViewComponents;
using Agility.NET.MVC.Starter.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Agility.NET.MVC.Starter.ViewComponents.Shared
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

            var siteHeaderResponse = await _fetchApiService.GetTypedContentList<Agility.Models.SiteHeader>(new GetListParameters()
            {
                ReferenceName = Constants.SiteHeaderReferenceName,
                Locale = locale,
                Take = 1,
                ContentLinkDepth = 0,
                IsPreview = Util.Helpers.PreviewHelpers.IsPreviewMode(HttpContext)

            });

            var siteHeader = siteHeaderResponse.Items.FirstOrDefault();
            if (siteHeader == null)
            {
                throw new System.ApplicationException($"SiteHeader with reference name {Constants.SiteHeaderReferenceName} not found.");
            }

            var sitemap = await _fetchApiService.GetTypedSitemapFlat(new GetSitemapParameters()
            {
                ChannelName = _appSettings.ChannelName,
                Locale = locale,
                IsPreview = Util.Helpers.PreviewHelpers.IsPreviewMode(HttpContext)
            });

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
