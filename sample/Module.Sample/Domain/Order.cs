using Luck.DDD.Domain;

namespace Module.Sample.Domain
{
    public class Order : FullAggregateRoot
    {
        public Order(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public string Name { get; private set; } = default!;

        public string Address { get; private set; } = default!;
    }
}
