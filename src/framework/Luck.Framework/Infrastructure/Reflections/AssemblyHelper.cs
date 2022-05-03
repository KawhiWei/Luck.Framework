using Microsoft.Extensions.DependencyModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Luck.Framework.Infrastructure
{
    public static class AssemblyHelper
    {
      
        /// <summary>
        /// 根据程序集名字得到程序集
        /// </summary>
        /// <param name="assemblyNames"></param>
        /// <returns></returns>

        public static IEnumerable<Assembly> GetAssembliesByName(params string[] assemblyNames)
        {
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath; //获取项目路径
            return assemblyNames.Select(o => AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(basePath, $"{o}.dll")));
        }

        private static readonly string[] Filters = { "dotnet-", "Microsoft.", "mscorlib", "netstandard", "System", "Windows" };

        private static Assembly[] _allAssemblies;
        private static Type[] _allTypes;

        /// <summary>
        /// 获取 所有程序集
        /// </summary>
        public static Assembly[] AllAssemblies
        {

            get {
                if (_allAssemblies == null)
                {
                    Init();
                }
                return _allAssemblies;
            }
        }

        /// <summary>
        /// 获取 所有类型
        /// </summary>
        public static Type[] AllTypes
        {

            get
            {
                if (_allTypes == null)
                {
                    Init();
                }
                return _allTypes;
            }
        }
        public static void Init()
        {

            _allAssemblies = DependencyContext.Default.GetDefaultAssemblyNames().Where(o => o.Name != null && !Filters.Any(o.Name.StartsWith)).Select(Assembly.Load).ToArray();
            _allTypes = _allAssemblies.SelectMany(m=>m.GetTypes()).ToArray();
        }

        /// <summary>
        /// 查找指定条件的类型
        /// </summary>
        public static Type[] FindTypes(Func<Type, bool> predicate)
        {
            return AllTypes.Where(predicate).ToArray();
        }

        public static Type[] FindTypesByAttribute<TAttribute>()
        
        {
            var attrbuteType = typeof(TAttribute);
            return FindTypesByAttribute(attrbuteType);
        }


        public static Type[] FindTypesByAttribute(Type type)
        {
            return AllTypes.Where(a=>a.IsDefined(type,true)).Distinct().ToArray();
        }
        /// <summary>
        /// 查找指定条件的类型
        /// </summary>
        public static Assembly[] FindAllItems(Func<Assembly, bool> predicate)
        {
            return AllAssemblies.Where(predicate).ToArray();
        }



    }

}