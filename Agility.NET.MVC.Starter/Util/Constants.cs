using System.Text.Json;

namespace Agility.NET.MVC.Starter.Util
{
    public class Constants
    {
        public static readonly string SiteHeaderReferenceName = "siteheader";
        public static readonly JsonSerializerOptions JsonSerializerOptions = Agility.NET.FetchAPI.Util.Constants.JsonSerializerOptions;
        public static readonly string BaseUrl = Agility.NET.FetchAPI.Util.Constants.BaseUrl;
        public static readonly string BaseUrlDev = Agility.NET.FetchAPI.Util.Constants.BaseUrlDev;
        public static readonly string Fetch = Agility.NET.FetchAPI.Util.Constants.Fetch;
        public static readonly string Preview = Agility.NET.FetchAPI.Util.Constants.Preview;
        public static readonly string Live = Agility.NET.FetchAPI.Util.Constants.Live;
        public static readonly string SitemapPagesKey = Agility.NET.FetchAPI.Util.Constants.SitemapPagesKey;
        public static readonly string UrlRedirectionsResponseKey = Agility.NET.FetchAPI.Util.Constants.UrlRedirectionsResponseKey;
        public static readonly string PageTypeFolder = Agility.NET.FetchAPI.Util.Constants.PageTypeFolder;
        public static readonly string AgilityPreviewKeyName = Agility.NET.FetchAPI.Util.Constants.AgilityPreviewKeyName;
        public static readonly string IsPreviewCookieName = Agility.NET.FetchAPI.Util.Constants.IsPreviewCookieName;
    }
}
