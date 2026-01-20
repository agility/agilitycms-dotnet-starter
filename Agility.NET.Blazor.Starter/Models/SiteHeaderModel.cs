using Agility.Models;
using Agility.NET.FetchAPI.Models.API;

namespace Agility.Net.Blazor.Starter.Models;

public class SiteHeaderModel
{
    public SiteHeader SiteHeader { get; set; } = new();
    public List<SitemapPage> SitemapPages { get; set; } = new();
}
