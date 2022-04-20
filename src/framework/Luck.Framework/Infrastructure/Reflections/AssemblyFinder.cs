using System.Reflection;

namespace Luck.Framework.Infrastructure
{
    public class AssemblyFinder : FinderBase<Assembly>, IAssemblyFinder
    {
        protected override Assembly[] FindAllItems()
        {
            return AssemblyHelper.FindAllItems();
        }
    }
}