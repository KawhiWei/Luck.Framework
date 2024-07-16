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
    
    public IPipelineDelegateBuilder<TContext> CreatePipelineDelegateBuilder<TContext>() where TContext : IContext
    {
        var pipelineBuilder = new PipelineDelegateBuilder<TContext>(_serviceProvider);
        return pipelineBuilder;
    }
}