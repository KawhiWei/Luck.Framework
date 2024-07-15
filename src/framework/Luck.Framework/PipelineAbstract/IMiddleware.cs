namespace Luck.Framework.PipelineAbstract;

public interface IMiddleware<TContext> where TContext : IContext
{
    IMiddleware<TContext>? NextMiddleware { get; set; }

    ValueTask ExecuteAsync(TContext context);
}