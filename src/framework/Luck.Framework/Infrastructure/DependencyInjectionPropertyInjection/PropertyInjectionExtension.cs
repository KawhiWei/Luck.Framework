using Microsoft.Extensions.Hosting;

namespace Luck.Framework.Infrastructure.DependencyInjectionPropertyInjection
{
    /// <summary>
    /// 属性注入扩展
    /// </summary>
    public static class PropertyInjectionExtension
    {
        /// <summary>
        /// 使用属性注入
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static void UseDefaultPropertyInjection(this IHostBuilder hostBuilder) => hostBuilder.UseServiceProviderFactory(new PropertyInjectionServiceProviderFactory());
    }
}
