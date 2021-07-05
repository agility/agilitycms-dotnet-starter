using System.Collections.Generic;
using Agility.Models;
using Agility.NET5.FetchAPI.Models.API;

namespace Agility.NET5.Starter.Models.ViewComponents
{
    public partial class SiteHeaderModel
    {
        public SiteHeader SiteHeader { get; set; }
        public List<SitemapPage> SitemapPages { get; set; }
    }
}
