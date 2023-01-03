﻿using Microsoft.Extensions.DependencyInjection;

namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 自定义配置服务上下文
    /// </summary>
    public class ConfigureServicesContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public ConfigureServicesContext(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// 
        /// </summary>
        public IServiceCollection Services { get; }
    }
}