namespace Luck.Framework.PipelineAbstract;

public interface IPipeline<in TContext> where TContext : IContext
{
    ValueTask InvokeAsync(TContext context);
}