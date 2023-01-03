using Luck.Framework.Extensions;
using Luck.Framework.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq.Expressions;

namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class ModuleApplicationBase : IModuleApplication
    {
        /// <summary>
        /// 
        /// </summary>
        public Type StartupModuleType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IServiceCollection Services { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 模块接口容器
        /// </summary>
        public IReadOnlyList<IAppModule> Modules { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<IAppModule> Source { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startupModuleType"></param>
        /// <param name="services"></param>
        public ModuleApplicationBase(Type startupModuleType, IServiceCollection services)
        {
            StartupModuleType = startupModuleType;
            Services = services;
      
            services.AddSingleton<IModuleApplication>(this);
            services.TryAddObjectAccessor<IServiceProvider>();
            Source = this.GetAllModule(services);
            Modules = this.LoadModules();
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        protected virtual List<IAppModule> GetAllModule(IServiceCollection services)
        {
            var typs = AssemblyHelper.FindTypes(o => AppModule.IsAppModule(o));
            var modules = typs.Select(o =>CreateModule(services, o)).Where(o=>o.IsNotNull()).Distinct();
            return modules.ToList();
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
                var dependModule = Source.Find(m => m.GetType() == dependType);
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
            var invoke=   Expression.Lambda(Expression.New(moduleType)).Compile().DynamicInvoke();


            if (invoke == null)
            {
                throw new ArgumentNullException(nameof(invoke));
            }
            var module = (IAppModule)invoke;

  
            if (module == null)
                throw new ArgumentNullException(nameof(module));

            if (module.Enable == false)
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

            Source?.Clear();
        }
    }
}