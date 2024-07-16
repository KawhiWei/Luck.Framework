using System.Collections.Generic;
using Luck.DDD.Domain.Domain.AggregateRoots;

namespace Luck.UnitTest.Models
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
}
