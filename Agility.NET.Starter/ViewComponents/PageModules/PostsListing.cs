using System.Threading.Tasks;
using Agility.Models;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Agility.NET.Starter.ViewComponents.PageModules
{
	public class PostsListing : ViewComponent
	{
		private readonly FetchApiService _fetchApiService;
		public PostsListing(FetchApiService fetchApiService)
		{
			_fetchApiService = fetchApiService;
		}

		public async Task<IViewComponentResult> InvokeAsync(ModuleModel moduleModel)
		{

			var posts = await _fetchApiService.GetContentByGraphQL<Post>(query: @"
				query {
					posts (take: 10, skip: 0, sort: ""fields.date"", direction: ""desc"" ) {
						contentID
						fields {
							title
							slug
							date
							image {
								label
								url
							}
							category {
								fields {
									title
								}
							}
						}
					}
				}",
				objName: "posts",
				locale: moduleModel.Locale,
				isPreview: Util.Helpers.PreviewHelpers.IsPreviewMode(HttpContext));

			// This is an alternative way to get the posts, using the GetTypedContentList method
			/*
			var posts = await _fetchApiService.GetTypedContentList<Post>(new GetListParameters
			{
				ReferenceName = "posts",
				Locale = moduleModel.Locale,
				IsPreview = Util.Helpers.PreviewHelpers.IsPreviewMode(HttpContext),
				ContentLinkDepth = 2,

			});
			*/
			return View("/Views/PageModules/PostsListing.cshtml", posts);
		}
	}
}
