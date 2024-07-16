namespace Luck.Framework.PipelineAbstract;

public interface IPipe<in TContext> where TContext : IContext
{
    ValueTask InvokeAsync(TContext context);
}