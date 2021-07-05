using System.Threading.Tasks;
using Agility.NET5.FetchAPI.Helpers;
using Agility.NET5.FetchAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET5.Starter.ViewComponents.PageModules
{
    public class TextBlockWithImage: ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(ModuleModel moduleModel)
        {
            var model = DynamicHelpers.DeserializeTo<Agility.Models.TextBlockWithImage>(moduleModel.Module);
            return Task.Run<IViewComponentResult>(() => View("/Views/PageModules/TextBlockWithImage.cshtml", model));
        }
    }
}
