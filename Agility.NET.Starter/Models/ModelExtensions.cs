using System.Collections.Generic;
using Agility.NET.FetchAPI.Models.API;

namespace Agility.Models
{
    public class ModelExtensions
    {
        public partial class FeaturedPostExpanded : FeaturedPost_Model
        {
            public ContentItemResponse<Post> FeaturedPost { get; set; }

            public new string FeaturedPost_ValueField { get; set; }

        }
        public partial class PostsListingExpanded : PostsListing
        {
            public new List<ContentItemResponse<Post>> Posts { get; set; }
        }
    }
}
