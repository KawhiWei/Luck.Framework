using Luck.DDD.Domain.Domain.Entities;

namespace Luck.UnitTest.Models;

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