using System.Threading.Tasks;
using Agility.NET5.FetchAPI.Helpers;
using Agility.NET5.FetchAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET5.Starter.ViewComponents.PageModules
{
    public class Heading: ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(ModuleModel moduleModel)
        {
            var model = DynamicHelpers.DeserializeTo<Agility.Models.Heading>(moduleModel.Module);
            return Task.Run<IViewComponentResult>(() => View("/Views/PageModules/Heading.cshtml", model));
        }
    }
}
