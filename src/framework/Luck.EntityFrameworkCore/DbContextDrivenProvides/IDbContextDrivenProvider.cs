using Luck.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luck.EntityFrameworkCore.DbContextDrivenProvides
{
    public interface IDbContextDrivenProvider
    {
        DataBaseType Type { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString"></param>
        /// <param name="querySplittingBehavior"></param>
        /// <typeparam name="TDbContext"></typeparam>
        /// <returns></returns>
        DbContextOptionsBuilder Builder<TDbContext>(DbContextOptionsBuilder builder, string connectionString,QuerySplittingBehavior querySplittingBehavior=QuerySplittingBehavior.SplitQuery) where TDbContext : ILuckDbContext;
    }
}
