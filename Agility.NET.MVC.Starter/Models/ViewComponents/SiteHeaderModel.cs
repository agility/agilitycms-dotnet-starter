using System.Collections.Generic;
using Agility.Models;
using Agility.NET.FetchAPI.Models.API;

namespace Agility.NET.MVC.Starter.Models.ViewComponents
{
    public partial class SiteHeaderModel
    {
        public SiteHeader? SiteHeader { get; set; }
        public List<SitemapPage> SitemapPages { get; set; } = new();
    }
}
