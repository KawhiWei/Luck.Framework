using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Luck.Framework.Extensions
{
    public static class JsonExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string Serialize<T>(this T obj, JsonSerializerOptions options)
        {
            if (obj == null)
                return string.Empty;
            return JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T? Deserialize<T>(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(text);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="options"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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
