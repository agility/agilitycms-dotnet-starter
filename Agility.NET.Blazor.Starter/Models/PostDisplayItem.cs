namespace Agility.NET.Blazor.Starter.Models;

/// <summary>
/// Display item that can hold either real post data or phantom data for infinite scroll demo
/// </summary>
public class PostDisplayItem
{
    public int ContentID { get; set; }
    public string? Title { get; set; }
    public string? CategoryTitle { get; set; }
    public string? Slug { get; set; }
    public string? Url { get; set; }
    public string? DateDisplay { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageLabel { get; set; }
    public bool IsPhantom { get; set; }
}
