using System.Collections.Generic;
using System.Threading.Tasks;
using Agility.Models;
using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models;
using Agility.NET.FetchAPI.Models.Data;
using Agility.NET.FetchAPI.Services;
using Agility.NET.Shared.Models;
using GraphQL;
using Microsoft.AspNetCore.Mvc;
using Agility.NET.FetchAPI.Models.API;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Agility.NET.Starter.ViewComponents.PageModules
{
    public class PostsListing: ViewComponent
    {
        private readonly FetchApiService _fetchApiService;
        public PostsListing(FetchApiService fetchApiService) { 
            _fetchApiService = fetchApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ModuleModel moduleModel)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                {
                    posts(take: 10, skip: 0)
                    {
                        contentID
                        fields {
                            title
                            slug
                            date
                            category {
                               fields {
                                title
                               }
                            }
                            image {
                                label
                                url
                                target
                                pixelHeight
                                pixelWidth
                                fileSize
                                height
                                width
                            }
                            content
                            authorID
                            categoryID
                        }
                    }
                }"
            };

            var rawData = await _fetchApiService.GetContentByGraphQL<PostGQL>(query, moduleModel.Locale, "posts");

            var posts = new List<PostGQL>();

            foreach (var post in rawData)
            {
                // just return the Fields
                posts.Add(post.Fields);
            }


            return View("/Views/PageModules/PostsListing.cshtml", posts);
        }
    }
}
