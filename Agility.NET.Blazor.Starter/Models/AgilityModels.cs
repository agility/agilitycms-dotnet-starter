using Agility.NET.FetchAPI.Models.API;

namespace Agility.Models;

public partial class RichTextArea
{
    public string TextBlob { get; set; } = string.Empty;
}

public partial class AgilityCSS
{
    public string Title { get; set; } = string.Empty;
    public string ReferenceName { get; set; } = string.Empty;
    public string TextBlob { get; set; } = string.Empty;
    public bool Minify { get; set; }
}

public partial class AgilityCodeTemplate
{
    public string Title { get; set; } = string.Empty;
    public string ReferenceName { get; set; } = string.Empty;
    public string TextBlob { get; set; } = string.Empty;
    public bool Visible { get; set; }
}

public partial class AgilityJavascript
{
    public string Title { get; set; } = string.Empty;
    public string ReferenceName { get; set; } = string.Empty;
    public string TextBlob { get; set; } = string.Empty;
    public bool Minify { get; set; }
}

public partial class PostsListing
{
    public List<Post> Posts { get; set; } = new();
}

public partial class PostDetails
{
}

public partial class Post
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public ContentItemResponse<Category>? Category { get; set; }
    public ImageAttachment? Image { get; set; }
    public string Content { get; set; } = string.Empty;
    public int AuthorID { get; set; }
    public string CategoryID { get; set; } = string.Empty;
}

public partial class SiteHeader
{
    public ImageAttachment? Logo { get; set; }
    public string SiteName { get; set; } = string.Empty;
}

public partial class Category
{
    public string Title { get; set; } = string.Empty;
}

public partial class TextBlockWithImage
{
    public string Tagline { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public ImageAttachment? Image { get; set; }
    public string ImagePosition { get; set; } = string.Empty;
    public Link? PrimaryButton { get; set; }
    public string Content { get; set; } = string.Empty;
    public string HighPriority { get; set; } = string.Empty;
}

public partial class FeaturedPost_Model
{
    public ContentItemResponse<Post>? featuredPost { get; set; }
    public string FeaturedPost_ValueField { get; set; } = string.Empty;
}

public partial class Heading
{
    public string Title { get; set; } = string.Empty;
}

public partial class ImageAttachment
{
    public string Label { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public object? Target { get; set; }
    public string PixelHeight { get; set; } = string.Empty;
    public string PixelWidth { get; set; } = string.Empty;
    public int? Filesize { get; set; }
    public int? Height { get; set; }
    public int? Width { get; set; }
}

public partial class Link
{
    public string Href { get; set; } = string.Empty;
    public string Target { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
