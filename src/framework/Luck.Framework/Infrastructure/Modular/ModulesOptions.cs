using Microsoft.Extensions.DependencyInjection;

namespace Luck.Framework.Infrastructure
{
    public class ModulesOptions
    {
        public IServiceCollection Service { get; }

        public ModulesOptions(IServiceCollection service)
        {
            Service = service;
        }
    }
}