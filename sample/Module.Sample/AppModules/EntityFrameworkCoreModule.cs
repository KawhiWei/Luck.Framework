using Luck.EntityFrameworkCore;
using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Module.Sample.DbContexts;

namespace Module.Sample
{
    public class EntityFrameworkCoreModule : EntityFrameworkCoreBaseModule
    {
        public override void AddDbContextWithUnitOfWork(IServiceCollection services)
        {
            services.AddLuckDbContext<ModuleDbContext>(x =>
            {
                x.ConnnectionString = "server=101.34.26.221;userid=root;pwd=&duyu789D;database=luck.sample;connectiontimeout=3000;port=40002;Pooling=true;Max Pool Size=300; Min Pool Size=5;";
                x.Type = DataBaseType.MySql;
            });
        }
    }
}
