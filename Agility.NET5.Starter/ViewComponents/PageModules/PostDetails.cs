using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET5.Starter.ViewComponents.PageModules
{
    public class PostDetails: ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string module)
        {
            return Task.Run<IViewComponentResult>(() => View("/Views/PageModules/PostDetails.cshtml"));
        }
    }
}
