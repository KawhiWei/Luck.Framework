using JetBrains.Annotations;
using Luck.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Luck.Framework.Utilities
{

    /// <summary>
    /// 检查
    /// </summary>
    public static class Check
    {

        /// <summary>
        /// 判断是否为NULL
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="value">动态类型值</param>
        /// <param name="parameterName">参数名</param>
        /// <returns>若NULL就抛异常，否则就返回本身</returns>
        public static T NotNull<T>([NotNull] T value, [InvokerParameterName] string parameterName)
        {

            value.NotNull(parameterName);
            return value;
        }


      
    }
}
