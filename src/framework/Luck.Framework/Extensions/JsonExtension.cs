using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Luck.Framework.Extensions
{
    public static class JsonExtension
    {
        public static string Serialize<T>(this T obj)
        {
            //https://q.cnblogs.com/q/115234/
            //https://github.com/dotnet/runtime/issues/28567
            var jsonOption = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            if (obj == null)
                return string.Empty;
            return JsonSerializer.Serialize(obj, jsonOption);
        }
        public static string Serialize<T>(this T obj, JsonSerializerOptions options)
        {
            if (obj == null)
                return string.Empty;
            return JsonSerializer.Serialize(obj, options);
        }

        public static T? Deserialize<T>(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(text);
        }
        public static T? Deserialize<T>(this string text, JsonSerializerOptions options)
        {
            if (string.IsNullOrEmpty(text))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(text, options);
        }

    }
}
