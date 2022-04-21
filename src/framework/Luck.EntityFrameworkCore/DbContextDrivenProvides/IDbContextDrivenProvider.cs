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

        DbContextOptionsBuilder Builder<TDbContext>(DbContextOptionsBuilder builder, string connectionString) where TDbContext : ILuckDbContext;
    }
}
