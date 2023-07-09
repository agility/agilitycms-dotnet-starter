using System.Threading.Tasks;
using Agility.NET5.FetchAPI.Models;
using Agility.NET5.FetchAPI.Models.Data;
using Agility.NET5.FetchAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Agility.Models;

namespace Agility.NET5.Starter.ViewComponents.PageModules
{
    public class PostDetails: ViewComponent
    {
        private readonly FetchApiService _fetchApiService;
        public PostDetails(FetchApiService fetchApiService)
        {
            _fetchApiService = fetchApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ModuleModel moduleModel)
        {
            var getParams = new GetItemParameters
            {
                ContentId = moduleModel.SitemapPage.ContentID,
                Locale = "en-us"
            };

            var post = await _fetchApiService.GetTypedContentItem<Post>(getParams);
            return View("/Views/PageModules/PostDetails.cshtml", post);
        }
    }
}
