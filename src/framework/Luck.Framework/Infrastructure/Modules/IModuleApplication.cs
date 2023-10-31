using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModuleApplication : IDisposable
    {
        
        /// <summary>
        /// 
        /// </summary>
        Type StartupModuleType { get; }
        
        /// <summary>
        /// 
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// 
        /// </summary>
        IServiceProvider? ServiceProvider { get; }

        /// <summary>
        /// 
        /// </summary>
        IReadOnlyList<IAppModule> Modules { get; }
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<IAppModule> Source { get; }
    }
}