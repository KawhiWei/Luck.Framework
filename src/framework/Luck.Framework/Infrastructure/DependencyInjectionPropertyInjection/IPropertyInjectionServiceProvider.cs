﻿using Microsoft.Extensions.DependencyInjection;

namespace Luck.Framework.Infrastructure.DependencyInjectionPropertyInjection
{
    /// <summary>
    /// 属性注入提供者
    /// </summary>
    public interface IPropertyInjectionServiceProvider : IServiceProvider, ISupportRequiredService
    {
        /// <summary>
        /// 判断注入属性
        /// </summary>
        /// <param name="instance"></param>
        public void IsInjectProperties(object instance);
    }
}
