using System.Threading.Tasks;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET.Starter.ViewComponents.PageModules
{

    public class Heading: ViewComponent
    {
        private readonly FetchApiService _fetchApiService;

        public Heading(FetchApiService fetchApiService)
        {
            _fetchApiService = fetchApiService;
        }
        public async Task<IViewComponentResult> InvokeAsync(ModuleModel moduleModel)
        {
            var getParams = new GetItemParameters
            {
                ContentId = moduleModel.Model.Item.ContentID,
                Locale = moduleModel.Locale
            };
            var heading = await _fetchApiService.GetTypedContentItem<Agility.Models.Heading>(getParams);
            return View("/Views/PageModules/Heading.cshtml", heading);
        }
    }
}
