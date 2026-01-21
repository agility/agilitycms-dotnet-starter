using System.Threading.Tasks;
using Agility.Models;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Services;
using Agility.NET.MVC.Starter.Util.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET.MVC.Starter.ViewComponents.PageModules
{
	public class FeaturedPost : ViewComponent
	{
		private readonly FetchApiService _fetchApiService;

		public FeaturedPost(FetchApiService fetchApiService)
		{
			_fetchApiService = fetchApiService;
		}

		public async Task<IViewComponentResult> InvokeAsync(ModuleModel moduleModel)
		{

			//get this component
			var component = await _fetchApiService.GetTypedContentItem<FeaturedPost_Model>(new GetItemParameters
			{
				ContentId = moduleModel.Model.Item.ContentID,
				Locale = moduleModel.Locale,
				IsPreview = Util.Helpers.PreviewHelpers.IsPreviewMode(HttpContext)
			});

			//get the content id of the featured post
			var featuredPostContentId = int.Parse(component?.Fields?.FeaturedPost_ValueField ?? "0");

			var featuredPost = await _fetchApiService.GetTypedContentItem<Post>(new GetItemParameters
			{
				ContentId = featuredPostContentId,
				Locale = moduleModel.Locale,
				IsPreview = Util.Helpers.PreviewHelpers.IsPreviewMode(HttpContext),
				ContentLinkDepth = 2
			});

			return View("/Views/PageModules/FeaturedPost.cshtml", featuredPost);
		}
	}
}
