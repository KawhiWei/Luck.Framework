using Luck.DDD.Domain;

namespace Luck.Walnut.Domain.AggregateRoots.Environments
{
    public class Configuration : FullEntity
    { 
    
        public string Key { get; private set; } = default!;

        public string Value { get; private set; } = default!;

        public string Type { get; private set; } = default!;
    }

}
