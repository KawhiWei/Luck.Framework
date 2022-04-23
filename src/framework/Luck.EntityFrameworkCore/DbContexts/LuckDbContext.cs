using Luck.EntityFrameworkCore.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.EntityFrameworkCore.DbContexts
{
    public class LuckDbContext : DbContextBase, ILuckDbContext
    {

        public LuckDbContext(DbContextOptions options, IServiceProvider serviceProvider) : base(options, serviceProvider)
        {
        }
    }
}
