using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using System.Runtime.Loader;
using Luck.Framework.Extensions;

namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssemblyHelper
    {
        private static readonly string[] Filters = { "dotnet-", "Microsoft.", "mscorlib", "netstandard", "System", "Windows" };

        // ReSharper disable once InconsistentNaming
        private static readonly IEnumerable<Assembly>? _allAssemblies;
        // ReSharper disable once InconsistentNaming
        private static readonly IEnumerable<Type>? _allTypes;
        
        /// <summary>
        /// 需要排除的项目
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private static readonly List<string> _filterLibs = new();
        
        /// <summary>
        /// 构造函数
        /// </summary>
        static AssemblyHelper()
        {
            _allAssemblies = DependencyContext.Default?.GetDefaultAssemblyNames().Where(c => c.Name is not null && !Filters.Any(c.Name.StartsWith) && !_filterLibs.Any(c.Name.StartsWith)).Select(Assembly.Load);
            _allTypes = _allAssemblies?.SelectMany(c => c.GetTypes());
        }
        
        /// <summary>
        /// 根据程序集名字得到程序集
        /// Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath; //获取项目路径
        /// </summary>
        /// <param name="assemblyNames"></param>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetAssembliesByName(params string[] assemblyNames)=>
            assemblyNames.Select(o => AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(Path.Combine(AppContext.BaseDirectory), $"{o}.dll")));
        
        /// <summary>
        /// 添加排除项目,该排除项目可能会影响AutoDependenceInjection自动注入,请使用的时候自行测试.
        /// </summary>
        /// <param name="names"></param>
        public static void AddExcludeLibs(params string[] names) => _filterLibs.AddRangeIfNotContains(names);
        
        /// <summary>
        /// 查找指定条件的类型
        /// </summary>
        public static IEnumerable<Type> FindTypes(Func<Type, bool> predicate)=>_allTypes!.Where(predicate).ToArray();
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public static Type[] FindTypesByAttribute<TAttribute>() where TAttribute : Attribute => FindTypesByAttribute(typeof(TAttribute));
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Type[] FindTypesByAttribute(Type type)=> _allTypes!.Where(a => a.IsDefined(type, true)).Distinct().ToArray();
        
        /// <summary>
        /// 查找指定条件的类型
        /// </summary>
        public static Assembly[] FindAllItems(Func<Assembly, bool> predicate)=> _allAssemblies!.Where(predicate).ToArray();
        
    }
}