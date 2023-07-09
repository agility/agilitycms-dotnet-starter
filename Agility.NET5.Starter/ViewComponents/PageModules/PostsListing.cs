using System.Threading.Tasks;
using Agility.Models;
using Agility.NET5.FetchAPI.Helpers;
using Agility.NET5.FetchAPI.Models;
using Agility.NET5.FetchAPI.Models.Data;
using Agility.NET5.FetchAPI.Services;
using Agility.NET5.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET5.Starter.ViewComponents.PageModules
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
