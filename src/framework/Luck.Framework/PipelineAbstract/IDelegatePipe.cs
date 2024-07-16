namespace Luck.Framework.PipelineAbstract;

public interface IDelegatePipe<TContext> where TContext : IContext
{
    ValueTask InvokeAsync(TContext context, DelegatePipe<TContext> next);
}