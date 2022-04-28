using Luck.EntityFrameworkCore.DbContextDrivenProvides;

namespace Luck.EntityFrameworkCore
{
    public class EFDbContextConfig
    {

        public string ConnnectionString { get; set; } = default!;

        public DataBaseType Type { get; set; }

        /// <summary>
        /// 迁移Assembly名字
        /// </summary>
        public string MigrationsAssemblyName { get; set; }

    }
}
