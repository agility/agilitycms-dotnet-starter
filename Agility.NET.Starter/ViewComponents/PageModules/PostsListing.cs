using System.Threading.Tasks;
using Agility.Models;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Services;
using Agility.NET.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET.Starter.ViewComponents.PageModules
{
    public class PostsListing: ViewComponent
    {
        private readonly FetchApiService _fetchApiService;
        public PostsListing(FetchApiService fetchApiService) { 
            _fetchApiService = fetchApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ModuleModel moduleModel)
        {
            var getParams = new GetListParameters
            {
                ReferenceName = "posts",
                Locale = moduleModel.Locale
            };
            var posts = await _fetchApiService.GetTypedContentList<Post>(getParams);
            return View("/Views/PageModules/PostsListing.cshtml", posts);
        }
    }
}
