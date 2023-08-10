using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET.Starter.ViewComponents.Shared
{
    public class PreviewBar: ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {

            return Task.Run<IViewComponentResult>(() => View("/Views/Shared/PreviewBar.cshtml"));
        }
    }
}
