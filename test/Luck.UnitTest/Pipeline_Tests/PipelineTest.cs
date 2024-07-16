using System;
using Luck.Framework.PipelineAbstract;
using Luck.Pipeline;
using Luck.UnitTest.Pipeline_Tests.Context;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Luck.UnitTest.Pipeline_Tests;

public class PipelineTest
{
    private readonly IServiceProvider _serviceProvider;

    public PipelineTest()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<IPipelineFactory, PipelineFactory>()
            .AddScoped<FetchOrderDetailMiddleware>()
            .AddScoped<CreateCustomerMiddleware>();
        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public void CreatePipelineFactory()
    {
        var pipelineFactory = _serviceProvider.GetService<IPipelineFactory>()!;
        var customerContext = new CustomerContext(Guid.NewGuid().ToString());

        var pipeline = pipelineFactory.CreatePipelineBuilder<CustomerContext>()
            .UseMiddleware<FetchOrderDetailMiddleware>()
            .UseMiddleware<CreateCustomerMiddleware>()
            .Build();

        pipeline.InvokeAsync(customerContext);
    }
    
    // [Fact]
    // public void CreatePipelineDelegateFactory()
    // {
    //     var pipelineFactory = _serviceProvider.GetService<IPipelineFactory>()!;
    //     var customerContext = new CustomerContext(Guid.NewGuid().ToString());
    //
    //     var pipeline = pipelineFactory.CreatePipelineBuilder<CustomerContext>()
    //         .UseMiddleware<FetchOrderDetailMiddleware>()
    //         .UseMiddleware<CreateCustomerMiddleware>()
    //         .Build();
    //
    //     pipeline.InvokeAsync(customerContext);
    // }
}