using Luck.Framework.Extensions;
using Luck.Framework.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Luck.AppModule
{
    /// <summary>
    /// 
    /// </summary>
    public class ModuleApplicationBase : IModuleApplication
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startupModuleType"></param>
        /// <param name="services"></param>
        protected ModuleApplicationBase(Type startupModuleType, IServiceCollection services)
        {
            ServiceProvider = null;
            StartupModuleType = startupModuleType;
            Services = services;
            _ = services.AddSingleton<IModuleApplication>(this);
            _ = services.TryAddObjectAccessor<IServiceProvider>();
            Source = GetEnabledAllModule(services);
            Modules = LoadModules;
        }

        /// <summary>
        /// 获取所有需要加载的模块
        /// </summary>
        /// <exception cref="Exception"></exception>
        private IReadOnlyList<IAppModule> LoadModules
        {
            get
            {
                var modules = new List<IAppModule>();
                var module = Source.FirstOrDefault(o => o.GetType() == StartupModuleType);
                if (module == null)
                {
                    throw new Exception($"类型为“{StartupModuleType.FullName}”的模块实例无法找到");
                }

                modules.Add(module);
                var dependedTypes = module.GetDependedTypes();
                foreach (var dependType in dependedTypes.Where(LuckAppModule.IsAppModule))
                {
                    var dependModule = Source.FirstOrDefault(o => o.GetType() == StartupModuleType) ?? throw new($"加载模块{module.GetType().FullName}时无法找到依赖模块{dependType.FullName}");
                    modules.AddIfNotContains(dependModule);
                }

                return modules;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type StartupModuleType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IServiceCollection Services { get; set; }

        /// <summary>
        /// IServiceProvider?
        /// </summary>
        public IServiceProvider? ServiceProvider { get; private set; }

        /// <summary>
        /// 模块接口容器
        /// </summary>
        public IReadOnlyList<IAppModule> Modules { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IAppModule> Source { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IEnumerable<IAppModule> GetEnabledAllModule(IServiceCollection services)
        {
            var findTypes = AssemblyHelper.FindTypes(LuckAppModule.IsAppModule);
            var modules = findTypes.Select(o => CreateModule(services, o)).Where(o => o is not null);
            return modules.Distinct()!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        protected virtual void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ServiceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value = ServiceProvider;
        }

        /// <summary>
        /// 创建模块
        /// </summary>
        /// <param name="services"></param>
        /// <param name="moduleType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        private static IAppModule? CreateModule(IServiceCollection services, Type moduleType)
        {
            var module = Expression.Lambda(Expression.New(moduleType)).Compile().DynamicInvoke() as IAppModule;
            ArgumentNullException.ThrowIfNull(module, nameof(moduleType));
            if (!module.Enable)
            {
                return null;
            }
            services.AddSingleton(moduleType, module);
            return module;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}