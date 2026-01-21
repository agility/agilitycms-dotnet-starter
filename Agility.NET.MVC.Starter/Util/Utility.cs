using System.Text.RegularExpressions;

namespace Agility.NET.MVC.Starter.Util
{
	public static class Utility
	{
		public static string CleanHtml(string html)
		{
			if (html == null) return "";
			var pattern = @"href=""~\/";
			var replacement = "href=\"/";
			return Regex.Replace(html, pattern, replacement);
		}
	}
}
