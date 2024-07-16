using Luck.Framework.PipelineAbstract;

namespace Luck.Pipeline;

public class DefaultPipeline<TContext> : List<IMiddleware<TContext>>,
    IPipe<TContext> where TContext : IContext
{
    public async ValueTask InvokeAsync(TContext context)
    {
        var middleware = this.FirstOrDefault();

        if (middleware != null)
        {
            await middleware.ExecuteAsync(context);
        }
    }
}