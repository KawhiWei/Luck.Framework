using Luck.EntityFrameworkCore.DbContextDrivenProvides;

namespace Luck.EntityFrameworkCore
{
    public class EFDbContextConfig
    {

        public string ConnnectionString { get; set; } = default!;

        public DataBaseType Type { get; set; }

    

    }
}
