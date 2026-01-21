using System.Threading.Tasks;
using Agility.NET.FetchAPI.Models;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Agility.Models;

namespace Agility.NET.MVC.Starter.ViewComponents.PageModules
{
	public class PostDetails : ViewComponent
	{
		private readonly FetchApiService _fetchApiService;
		public PostDetails(FetchApiService fetchApiService)
		{
			_fetchApiService = fetchApiService;
		}

		public async Task<IViewComponentResult> InvokeAsync(ModuleModel moduleModel)
		{


			var post = await _fetchApiService.GetTypedContentItem<Post>(new GetItemParameters
			{
				ContentId = moduleModel.SitemapPage.ContentID,
				Locale = "en-us",
				IsPreview = Util.Helpers.PreviewHelpers.IsPreviewMode(HttpContext),
				ContentLinkDepth = 3,
			});
			return View("/Views/PageModules/PostDetails.cshtml", post);
		}
	}
}
