namespace Agility.NET.Starter.Util.Helpers
{
    public static class PageHelpers
    {
        public static string GetPageTemplatePath(string templateName)
        {
            return $"/Views/PageTemplates/{templateName.Replace(" ", string.Empty)}.cshtml";
        }
    }
}
