using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Agility.NET.FetchAPI.Helpers;
using Agility.NET.FetchAPI.Models.API;
using Agility.NET.FetchAPI.Services;
using Agility.NET.Starter.Util.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Agility.NET.Starter.Util.Middleware
{
	public class AgilityRedirectMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly FetchApiService _fetchApiService;
		private readonly AppSettings _appSettings;
		private readonly IMemoryCache _cache;
		private readonly IWebHostEnvironment _env;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AgilityRedirectMiddleware(RequestDelegate next,
			 FetchApiService fetchApiService,
			 IMemoryCache cache,
			 IWebHostEnvironment env,
			 IOptions<AppSettings> appSettings,
			 IHttpContextAccessor httpContextAccessor)
		{
			_next = next;
			_fetchApiService = fetchApiService;
			_cache = cache;
			_env = env;
			_httpContextAccessor = httpContextAccessor;
			_appSettings = appSettings.Value;
		}

		public async Task InvokeAsync(HttpContext context)
		{

			var sitemapPages =
				 await TransformerMiddlewareHelpers.GetOrCacheSitemapPages(
					  _env, _appSettings, _fetchApiService, _httpContextAccessor, _cache);

			var urlRedirectsResponse =
				 await TransformerMiddlewareHelpers.GetOrCacheUrlRedirects(
				 _env, _appSettings, _fetchApiService, _httpContextAccessor, _cache);


			if (context.Request.Path.Value != null)
			{
				var path = context.Request.Path.Value.TrimStart('/');

				if (CheckUrlRedirects(context, urlRedirectsResponse, path)) return;

				if (CheckIfDifferentPageNameInLocale(context, sitemapPages, path)) return;
			}

			await _next(context);
		}

		private static bool CheckIfDifferentPageNameInLocale(HttpContext context, List<SitemapPage> sitemapPages, string path)
		{
			var agilityPage =
				 sitemapPages
					  .Find(s => TransformerMiddlewareHelpers.DoesAgilityPageExist(s, path));

			if (agilityPage != null) return false;

			agilityPage = TransformerMiddlewareHelpers.CheckLocaleWithDifferentPageName(path, sitemapPages);

			if (agilityPage == null) return false;

			context.Response.Redirect(
				 $"{context.Request.Scheme}://{context.Request.Host}/{agilityPage.Locale}{agilityPage.Path}", true);

			return true;

		}

		private static bool CheckUrlRedirects(HttpContext context, UrlRedirectionsResponse urlRedirectsResponse, string path)
		{
			try
			{
				if (urlRedirectsResponse == null) return false;

				if (!urlRedirectsResponse.Items.Any()) return false;

				foreach (var urlRedirect in urlRedirectsResponse.Items)
				{
					var originUrlCleaned = urlRedirect.OriginUrl.StartsWith('~')
						 ? urlRedirect.OriginUrl.TrimStart('~')
							  .TrimStart('/')
						 : urlRedirect.OriginUrl;

					if (path != originUrlCleaned) continue;

					var destinationUrlCleaned = urlRedirect.DestinationUrl.StartsWith('~')
						 ? urlRedirect.DestinationUrl.TrimStart('~')
							  .TrimStart('/')
						 : urlRedirect.DestinationUrl;

					context.Response.Redirect(destinationUrlCleaned, true);
					return true;
				}

				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
