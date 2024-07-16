using System;
using System.Threading.Tasks;
using Luck.Pipeline;
using Luck.UnitTest.Pipeline_Tests.Context;

namespace Luck.UnitTest.Pipeline_Tests;

public class FetchOrderDetailPipe : DefaultPipe<CustomerContext>
{
    protected override ValueTask Invoke(CustomerContext context)
    {
        Console.WriteLine($"添加测试数据");
        context.Properties.Add("测试数据", "测试订单列表");
        return new ValueTask();
    }
}

public class CreateCustomerPipe : DefaultPipe<CustomerContext>
{
    protected override ValueTask Invoke(CustomerContext context)
    {
        if (context.Properties.TryGetValue("测试数据", out var value))
        {
            Console.WriteLine($"测试输出信息：【{value}】");
        }

        ;
        return new ValueTask();
    }
}