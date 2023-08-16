using Luck.DDD.Domain;
using Luck.DDD.Domain.Domain.AggregateRoots;
using Luck.DDD.Domain.Domain.Entities;

namespace Module.Sample.Domain
{
    public class Order : FullAggregateRoot
    {
        public Order(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public string Name { get; private set; }

        public string Address { get; private set; }

        public ICollection<OrderItem> OrderItems { get; private set; } = new HashSet<OrderItem>();

        public Order SetOrderItem()
        {
            OrderItems = new List<OrderItem>();
            for (var i = 0; i < 10; i++)
            {
                OrderItems.Add(new OrderItem($"性能优化00{i}","国家政治中心"));
            }
            return this;
        }

    }
    
    public class OrderItem : FullEntity
    {
        public OrderItem(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public string Name { get; private set; }

        public string Address { get; private set; }

        public Order Order { get; private set; } = default!;

    }
}
