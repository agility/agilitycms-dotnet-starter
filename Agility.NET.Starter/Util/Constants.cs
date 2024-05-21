using System.Text.Json;

namespace Agility.NET.Starter.Util
{
    public class Constants
    {
        public static readonly string SiteHeaderReferenceName = "siteheader";
        public static readonly JsonSerializerOptions JsonSerializerOptions = Shared.Util.Constants.JsonSerializerOptions;
        public static readonly string BaseUrl = Shared.Util.Constants.BaseUrl;
        public static readonly string BaseUrlDev = Shared.Util.Constants.BaseUrlDev;
        public static readonly string Fetch = Shared.Util.Constants.Fetch;
        public static readonly string Preview = Shared.Util.Constants.Preview;
        public static readonly string Live = Shared.Util.Constants.Live;
        public static readonly string SitemapPagesKey = Shared.Util.Constants.SitemapPagesKey;
        public static readonly string UrlRedirectionsResponseKey = Shared.Util.Constants.UrlRedirectionsResponseKey;
        public static readonly string PageTypeFolder = Shared.Util.Constants.PageTypeFolder;
        public static readonly string AgilityPreviewKeyName = Shared.Util.Constants.AgilityPreviewKeyName;
        public static readonly string IsPreviewCookieName = Shared.Util.Constants.IsPreviewCookieName;
    }
}
