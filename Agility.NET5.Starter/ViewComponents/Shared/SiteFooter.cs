using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET5.Starter.ViewComponents.Shared
{
    public class SiteFooter: ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {

            return Task.Run<IViewComponentResult>(() => View("/Views/Shared/SiteFooter.cshtml"));
        }
    }
}
