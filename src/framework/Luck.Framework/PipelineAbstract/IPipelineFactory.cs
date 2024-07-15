namespace Luck.Framework.PipelineAbstract;

public interface IPipelineFactory
{
    IPipelineBuilder<TContext> CreatePipelineBuilder<TContext>() where TContext : IContext;
}