using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Models.Data;

namespace Agility.Net.Blazor.Starter.Models;

public class AgilityPageModel
{
    public PageResponse? PageResponse { get; set; }
    public List<ContentZone> ContentZones { get; set; } = new();
    public string Locale { get; set; } = string.Empty;
    public SitemapPage? SitemapPage { get; set; }
    public bool IsPreview { get; set; }
    public string? ContentItem { get; set; }
}
