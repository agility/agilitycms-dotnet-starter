using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Agility.NET.Shared.Models;
using Agility.NET.Shared.Util;
using RestSharp;

namespace Agility.NET.AgilityCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //update agility-models
                if (args.Length <= 0) return;

                if (args[0] != "update") return;

                Console.ForegroundColor = ConsoleColor.Green;

                var instanceGUID = ConfigurationManager.AppSettings["InstanceGUID"];
                var websiteName = ConfigurationManager.AppSettings["WebsiteName"];
                var fetchAPIKey = ConfigurationManager.AppSettings["FetchAPIKey"];
                var previewAPIKey = ConfigurationManager.AppSettings["PreviewAPIKey"];
                var path = ConfigurationManager.AppSettings["Path"];

                var apiType = string.IsNullOrEmpty(args[1]) ? Constants.Preview : args[1];
                var apiKey = apiType == Constants.Preview ? previewAPIKey : fetchAPIKey;


                var client = new RestClient($"{(instanceGUID != null && instanceGUID.EndsWith("-d") ? Constants.BaseUrlDev : Constants.BaseUrl)}/{instanceGUID}/{apiType}");
                client.AddDefaultHeader("accept", "application/json");
                client.AddDefaultHeader("APIKey", apiKey ?? string.Empty);

                Console.WriteLine($@"Downloading content definitions from Agility website: {websiteName} GUID: {instanceGUID}...");
                Console.ForegroundColor = ConsoleColor.Blue;

                var request = new RestRequest("/contentmodels", Method.Get);
                var contentDefinitions = client.Execute<Dictionary<string, ContentDefinition>>(request).Data;

                var builder = GenerateClasses(contentDefinitions, out var errors);

                if (!string.IsNullOrEmpty(errors))
                {
                    throw new ApplicationException($@"Error generating classes: {errors}");
                }

                File.WriteAllText(path ?? string.Empty, builder.ToString());

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($@"Created file: {path}");
                Console.ResetColor();

            }
            catch (Exception ex)
            {
                LogErrorMessage(ex);
            }
        }

        private static void LogErrorMessage(Exception ex)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($@"Error occurred running agility-sync {ex.Message}");
            Console.ResetColor();
        }

        private static StringBuilder GenerateClasses(Dictionary<string, ContentDefinition> contentDefinitions, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                var builder = new StringBuilder();
                builder.Append("using System;");
                builder.AppendLine();
                builder.Append("using System.Collections.Generic;");
                builder.AppendLine();
                builder.AppendLine();
                builder.Append("namespace Agility.Models {");
                builder.AppendLine();
                builder.AppendLine();

                foreach (var (_, value) in contentDefinitions)
                {

                    var className = value.ReferenceName;
                    var exists = value.Fields.FirstOrDefault(f => f.Name == value.ReferenceName);
                    if (exists != null)
                    {
                        className = $"{className}_Model";
                    }

                    builder.AppendLine($@"public partial class {className}");
                    builder.AppendLine("{");

                    foreach (var field in value.Fields)
                    {
                        var dataType = GetDataTypeFromValue(field.Type, field.Settings, contentDefinitions);
                        builder.AppendLine($"   public {dataType} {field.Name} {{ get; set; }}\r\n");
                    }
                    builder.AppendLine("}");
                }

                builder.AppendLine();
                builder.AppendLine($@"public partial class ImageAttachment");
                builder.AppendLine("{");
                builder.AppendLine("    public string Label { get; set; }");
                builder.AppendLine("    public string Url { get; set; }");
                builder.AppendLine("    public object Target { get; set; }");
                builder.AppendLine("    public string PixelHeight { get; set; }");
                builder.AppendLine("    public string PixelWidth { get; set; }");
                builder.AppendLine("    public int? Filesize { get; set; }");
                builder.AppendLine("    public int? Height { get; set; }");
                builder.AppendLine("    public int? Width { get; set; }");
                builder.AppendLine("}");
                builder.AppendLine();

                builder.AppendLine($@"public partial class Link");
                builder.AppendLine("{");
                builder.AppendLine("    public string Href { get; set; }");
                builder.AppendLine("    public string Target { get; set; }");
                builder.AppendLine("    public string Text { get; set; }");
                builder.AppendLine("}");
                builder.AppendLine();
                builder.AppendLine("}");

                return builder;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + Environment.NewLine + ex.StackTrace;
                return new StringBuilder();
            }
        }

        private static string GetDataTypeFromValue(string type,
            Setting setting,
            Dictionary<string, ContentDefinition> contentDefinitions)
        {
            if (contentDefinitions == null) throw new ArgumentNullException(nameof(contentDefinitions));

            switch (type.ToLower())
            {
                case Constants.Html:
                case Constants.Text:
                case Constants.CustomField:
                case Constants.DropDownList:
                case Constants.LongText:
                    return "string";
                case Constants.Boolean:
                    return "bool";
                case Constants.Link:
                    return "Link";
                case Constants.Content:
                    contentDefinitions.TryGetValue(setting.ContentDefinition, out var contentDefinition);
                    return setting.RenderAs == "dropdown" ? $"{contentDefinition?.ReferenceName}" : $"List<{contentDefinition?.ReferenceName}>";
                case Constants.Date:
                    return "DateTime";
                case Constants.ImageAttachment:
                    return "ImageAttachment";
                case Constants.Integer:
                    return "int";
                default:
                    return null;
            }
        }
    }
}
