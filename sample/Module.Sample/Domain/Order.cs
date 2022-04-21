using Luck.Framework.Domian;

namespace Module.Sample.Domain
{
    public class Order: FullAggregateRoot
    {

        public string Name { get; set; } = default!;

        public string Address { get; set; } = default!;
    }
}
