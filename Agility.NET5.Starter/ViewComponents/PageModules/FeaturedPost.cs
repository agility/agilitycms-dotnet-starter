using System.Threading.Tasks;
using Agility.Models;
using Agility.NET5.FetchAPI.Helpers;
using Agility.NET5.FetchAPI.Models;
using Agility.NET5.FetchAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET5.Starter.ViewComponents.PageModules
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
            var model = DynamicHelpers.DeserializeTo<ModelExtensions.FeaturedPostExpanded>(moduleModel.Module);
            return await Task.Run<IViewComponentResult>(() => View("/Views/PageModules/FeaturedPost.cshtml", model.FeaturedPost.Fields));
        }
    }
}
