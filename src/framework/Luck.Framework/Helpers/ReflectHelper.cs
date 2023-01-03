using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Luck.Framework.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReflectHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Assembly[] GetAssemblies()
        {
            Assembly[]? assemblies = null;
#if NET45
            if (System.Web.Hosting.HostingEnvironment.IsHosted)
            {
                assemblies = System.Web.Compilation.BuildManager.GetReferencedAssemblies()
                                            .Cast<Assembly>().ToArray();
            }
#endif

            if (null == assemblies || assemblies.Length == 0)
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }

            return assemblies ?? ArrayHelper.Empty<Assembly>();
        }
    }
}
