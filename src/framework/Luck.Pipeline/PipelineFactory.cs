using Luck.Framework.PipelineAbstract;

namespace Luck.Pipeline;

public class PipelineFactory : IPipelineFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PipelineFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    public IPipelineBuilder<TContext> CreatePipelineBuilder<TContext>() where TContext : IContext
    {
        var pipelineBuilder = new PipelineBuilder<TContext>(_serviceProvider);
        return pipelineBuilder;
    }
    
    public IDelegatePipelineBuilder<TContext> CreateDelegatePipelineBuilder<TContext>() where TContext : IContext
    {
        var pipelineBuilder = new DelegatePipelineBuilder<TContext>(_serviceProvider);
        return pipelineBuilder;
    }
}