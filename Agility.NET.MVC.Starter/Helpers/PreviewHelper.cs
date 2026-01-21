using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Agility.NET.MVC.Starter.Util.Helpers
{
	public static class PreviewHelpers
	{
		public static bool IsPreviewMode(IHttpContextAccessor httpContextAccessor)
		{
			if (httpContextAccessor.HttpContext == null) return false;

			var httpContext = httpContextAccessor.HttpContext;

			return httpContext.Items["IsPreview"] is bool isPreview && isPreview;
		}

		public static bool IsPreviewMode(HttpContext httpContext)
		{

			return httpContext.Items["IsPreview"] is bool isPreview && isPreview;
		}

		public static void SetPreviewMode(IHttpContextAccessor httpContextAccessor, bool isPreview)
		{
			if (httpContextAccessor.HttpContext == null) return;

			var httpContext = httpContextAccessor.HttpContext;

			httpContext.Items["IsPreview"] = isPreview;
		}

		public static void SetPreviewCookie(IHttpContextAccessor httpContextAccessor)
		{
			if (httpContextAccessor.HttpContext == null) return;

			var httpContext = httpContextAccessor.HttpContext;

			httpContext.Response.Cookies.Delete("isPreview");
			httpContext.Response.Cookies.Append(
				 Constants.IsPreviewCookieName,
				 "true",
				 new CookieOptions()
				 {
					 Path = "/"
				 }
			);
		}

		public static bool CheckPreviewCookie(IHttpContextAccessor httpContextAccessor)
		{
			if (httpContextAccessor.HttpContext == null) return false;

			var isPreview = httpContextAccessor.HttpContext.Request.Cookies[Constants.IsPreviewCookieName]?.ToLower();

			if (!string.IsNullOrEmpty(isPreview)) return isPreview == "true";

			httpContextAccessor.HttpContext.Response.Headers.TryGetValue("Set-Cookie", out var responseHeader);
			var setCookie = responseHeader.FirstOrDefault();

			if (setCookie == null) return isPreview == "true";

			if (setCookie.Contains($@"{Constants.IsPreviewCookieName}=true"))
			{
				isPreview = "true";
			}

			return isPreview == "true";
		}

	}
}