namespace Luck.Framework.PipelineAbstract;

public interface IPipe<TContext> where TContext : IContext
{
    IPipe<TContext>? NextPipe { get; set; }

    ValueTask ExecuteAsync(TContext context);
}