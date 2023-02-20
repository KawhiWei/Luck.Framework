using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Microsoft.EntityFrameworkCore;

namespace Luck.EntityFrameworkCore
{
    public class EfDbContextConfig
    {

        public string ConnectionString { get; set; } = default!;

        public DataBaseType Type { get; set; }

        public QuerySplittingBehavior QuerySplittingBehavior { get; set; } = QuerySplittingBehavior.SplitQuery;



    }
}
