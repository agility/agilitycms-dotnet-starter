using System.Threading.Tasks;
using Agility.NET5.FetchAPI.Helpers;
using Agility.NET5.FetchAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET5.Starter.ViewComponents.PageModules
{
    public class RichTextArea : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(ModuleModel moduleModel)
        {
            var module = DynamicHelpers.DeserializeTo<Agility.Models.RichTextArea>(moduleModel.Module);
            return Task.Run<IViewComponentResult>(() => View("/Views/PageModules/RichTextArea.cshtml", module));
        }
    }
}
