using System;
using Luck.Framework.PipelineAbstract;
using Luck.Pipeline;
using Luck.UnitTest.Pipeline_Tests.Context;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Luck.UnitTest.Pipeline_Tests;

public class PipelineTest
{
    [Fact]
    public void CreatePipelineFactory()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<IPipelineFactory, PipelineFactory>()
            .AddScoped<FetchOrderDetailPipe>()
            .AddScoped<CreateCustomerPipe>();
        var serviceProvider = services.BuildServiceProvider();
        var pipelineFactory = serviceProvider.GetService<IPipelineFactory>()!;
        var customerContext = new CustomerContext(Guid.NewGuid().ToString());

        var actuator = pipelineFactory.CreatePipelineBuilder<CustomerContext>()
            .UseMiddleware<FetchOrderDetailPipe>()
            .UseMiddleware<CreateCustomerPipe>()
            .Build();

        actuator.InvokeAsync(customerContext);
    }

    [Fact]
    public void CreateDelegatePipelineFactory()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<IPipelineFactory, PipelineFactory>()
            .AddScoped<FetchOrderDetailDelegatePipePipe>()
            .AddScoped<CreateCustomerDelegatePipe>()
            .AddScoped<CancelDelegatePipe>();
        var serviceProvider = services.BuildServiceProvider();
        var pipelineFactory = serviceProvider.GetService<IPipelineFactory>()!;

        var fetchOrderDetailDelegatePipePipe = serviceProvider.GetService<FetchOrderDetailDelegatePipePipe>()!;
        var createCustomerDelegatePipe = serviceProvider.GetService<CreateCustomerDelegatePipe>()!;
        var cancelDelegatePipe = serviceProvider.GetService<CancelDelegatePipe>()!;


        var customerContext = new CustomerContext(Guid.NewGuid().ToString());

        var actuator = pipelineFactory.CreateDelegatePipelineBuilder<CustomerContext>()
            .UsePipe(fetchOrderDetailDelegatePipePipe)
            .UsePipe(cancelDelegatePipe)
            .UsePipe(createCustomerDelegatePipe)
            .Build();
        actuator.Invoke(customerContext);
    }
}