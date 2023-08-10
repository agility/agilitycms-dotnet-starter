using System;
using System.Collections.Generic;

namespace Agility.Models {

public partial class RichTextArea
{
   public string TextBlob { get; set; }

}
public partial class AgilityCSS
{
   public string Title { get; set; }

   public string ReferenceName { get; set; }

   public string TextBlob { get; set; }

   public bool Minify { get; set; }

}
public partial class AgilityCodeTemplate
{
   public string Title { get; set; }

   public string ReferenceName { get; set; }

   public string TextBlob { get; set; }

   public bool Visible { get; set; }

}
public partial class AgilityJavascript
{
   public string Title { get; set; }

   public string ReferenceName { get; set; }

   public string TextBlob { get; set; }

   public bool Minify { get; set; }

}
public partial class PostsListing
{
   public List<Post> Posts { get; set; }

}
public partial class PostDetails
{
}
public partial class Post
{
   public string Title { get; set; }

   public string Slug { get; set; }

   public DateTime Date { get; set; }

   public Category Category { get; set; }

   public ImageAttachment Image { get; set; }

   public string Content { get; set; }

   public int AuthorID { get; set; }

   public string CategoryID { get; set; }

}
public partial class SiteHeader
{
   public ImageAttachment Logo { get; set; }

   public string SiteName { get; set; }

}
public partial class Category
{
   public string Title { get; set; }

}
public partial class TextBlockWithImage
{
   public string Tagline { get; set; }

   public string Title { get; set; }

   public ImageAttachment Image { get; set; }

   public string ImagePosition { get; set; }

   public Link PrimaryButton { get; set; }

   public string Content { get; set; }

}
public partial class FeaturedPost_Model
{
   public Post FeaturedPost { get; set; }

   public string FeaturedPost_ValueField { get; set; }

}
public partial class Heading
{
   public string Title { get; set; }

}

public partial class ImageAttachment
{
    public string Label { get; set; }
    public string Url { get; set; }
    public object Target { get; set; }
    public string PixelHeight { get; set; }
    public string PixelWidth { get; set; }
    public int? Filesize { get; set; }
    public int? Height { get; set; }
    public int? Width { get; set; }
}

public partial class Link
{
    public string Href { get; set; }
    public string Target { get; set; }
    public string Text { get; set; }
}

}
