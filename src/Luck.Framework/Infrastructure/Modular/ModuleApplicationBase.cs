using Luck.Framework.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq.Expressions;

namespace Luck.Framework.Infrastructure
{
    public class ModuleApplicationBase : IModuleApplication
    {
        public Type StartupModuleType { get; set; }

        public IServiceCollection Services { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 模块接口容器
        /// </summary>
        public IReadOnlyList<IAppModule> Modules { get; set; }

        public List<IAppModule> Source { get; protected set; }

        public ModuleApplicationBase(Type startupModuleType, IServiceCollection services)
        {
            StartupModuleType = startupModuleType;
            Services = services;
            services.TryAddSingleton<IAssemblyFinder, AssemblyFinder>();
            services.TryAddSingleton<ITypeFinder, TypeFinder>();
            services.AddSingleton<IModuleApplication>(this);
            services.TryAddObjectAccessor<IServiceProvider>();
            Source = this.GetAllModule(services);
            Modules = this.LoadModules();
        }

        protected virtual List<IAppModule> GetAllModule(IServiceCollection services)
        {
            var typeFinder = services.GetOrAddSingletonService<ITypeFinder, TypeFinder>();
            var typs = typeFinder.Find(o => AppModule.IsAppModule(o));
            var modules = typs.Select(o => CreateModule(services, o)).Distinct();
            return modules.ToList();
        }

        protected virtual void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ServiceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value = ServiceProvider;
        }

        /// <summary>
        /// 获取所有需要加载的模块
        /// </summary>
        /// <returns></returns>
        protected virtual IReadOnlyList<IAppModule> LoadModules()
        {
            List<IAppModule> modules = new List<IAppModule>();

            var module = Source.FirstOrDefault(o => o.GetType() == StartupModuleType);
            if (module == null)
            {
                throw new Exception($"类型为“{StartupModuleType.FullName}”的模块实例无法找到");
            }
            modules.Add(module);
            var dependeds = module.GetDependedTypes();
            foreach (var dependType in dependeds.Where(o => AppModule.IsAppModule(o)))
            {
                var dependModule = Source.ToList().Find(m => m.GetType() == dependType);
                if (dependModule == null)
                {
                    throw new Exception($"加载模块{module.GetType().FullName}时无法找到依赖模块{dependType.FullName}");
                }
                modules.AddIfNotContains(dependModule);
            }
            return modules;
        }

        /// <summary>
        /// 创建模块
        /// </summary>
        /// <param name="services"></param>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        private IAppModule CreateModule(IServiceCollection services, Type moduleType)
        {
            var module = (IAppModule)Expression.Lambda(Expression.New(moduleType)).Compile().DynamicInvoke();
            if (module == null)
                throw new ArgumentNullException(nameof(module));
            services.AddSingleton(moduleType, module);
            return module;
        }

        public virtual void Dispose()
        {
        }
    }
}