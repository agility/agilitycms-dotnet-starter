using System.Threading.Tasks;
using Agility.NET5.FetchAPI.Models.API;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET5.Starter.ViewComponents.Shared
{
    public class SEO: ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PageResponse pageResponse)
        {

            return Task.Run<IViewComponentResult>(() => View("/Views/Shared/SEO.cshtml", pageResponse));
        }
    }
}
