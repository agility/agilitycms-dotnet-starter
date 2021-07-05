using System.Threading.Tasks;
using Agility.Models;
using Agility.NET5.FetchAPI.Helpers;
using Agility.NET5.FetchAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET5.Starter.ViewComponents.PageModules
{
    public class PostsListing: ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(ModuleModel moduleModel)
        {
            var postsListing = DynamicHelpers.DeserializeTo<ModelExtensions.PostsListingExpanded>(moduleModel.Module);
            return Task.Run<IViewComponentResult>(() => View("/Views/PageModules/PostsListing.cshtml", postsListing));
        }
    }
}
