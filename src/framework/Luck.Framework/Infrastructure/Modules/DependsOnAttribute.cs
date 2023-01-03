using System;

namespace Luck.Framework.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependsOnAttribute : Attribute, IDependedTypesProvider
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dependedTypes"></param>
        public DependsOnAttribute(params Type[] dependedTypes)
        {
            DependedTypes = dependedTypes ?? Type.EmptyTypes;
        }

        /// <summary>
        /// 依赖类型集合
        /// </summary>
        private Type[] DependedTypes { get; }

        /// <summary>
        /// 得到依赖类型集合
        /// </summary>
        /// <returns></returns>
        public Type[] GetDependedTypes()
        {
            return DependedTypes;
        }
    }
}