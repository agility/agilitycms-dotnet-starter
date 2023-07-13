using System.Threading.Tasks;
using Agility.Models;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET.Starter.ViewComponents.PageModules
{
    public class FeaturedPost: ViewComponent
    {
        private readonly FetchApiService _fetchApiService;

        public FeaturedPost(FetchApiService fetchApiService)
        {
            _fetchApiService = fetchApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ModuleModel moduleModel)
        {
            var getParams = new GetItemParameters
            {
                ContentId = moduleModel.Model.Item.ContentID,
                Locale = moduleModel.Locale
            };

            var featuredPostModel = await _fetchApiService.GetTypedContentItem<FeaturedPost_Model>(getParams);

            var featuredPostContentId = int.Parse(featuredPostModel.Fields.FeaturedPost_ValueField);
            
            getParams.ContentId = featuredPostContentId;

            var featuredPost = await _fetchApiService.GetTypedContentItem<Post>(getParams);

            return View("/Views/PageModules/FeaturedPost.cshtml", featuredPost);
        }
    }
}
