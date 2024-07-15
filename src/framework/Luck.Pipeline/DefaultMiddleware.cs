using Luck.Framework.PipelineAbstract;

namespace Luck.Pipeline;

public abstract class DefaultMiddleware<TContext> : IMiddleware<TContext> where TContext : IContext
{
    public IMiddleware<TContext>? NextMiddleware { get; set; } = null;

    public async ValueTask ExecuteAsync(TContext context)
    {
        try
        {
            var runThis = await BeforeInvokeAsync(context);
            if (context.IsInterruptible)
            {
                await OnCancelled(context);
                return;
            }

            if (runThis)
            {
                await Invoke(context);
            }

            if (context.IsInterruptible)
            {
                await OnCancelled(context);
                return;
            }

            if (NextMiddleware is not null)
            {
                await NextMiddleware.ExecuteAsync(context);
            }


            if (context.IsInterruptible)
            {
                await OnCancelled(context);
                return;
            }

            if (runThis)
            {
                await AfterInvokeAsync(context);
            }

            if (context.IsInterruptible)
            {
                await OnCancelled(context);
            }
        }
        catch (Exception ex)
        {
            await OnCancelled(context, ex);
        }
    }

    protected abstract ValueTask Invoke(TContext context);

    protected virtual ValueTask<bool> BeforeInvokeAsync(TContext context) => ValueTask.FromResult(true);

    protected virtual ValueTask AfterInvokeAsync(TContext context) => ValueTask.CompletedTask;

    protected virtual ValueTask OnCancelled(TContext context, Exception ex = null!)
    {
        if (ex != null)
        {
            throw new Exception(this.GetType().Name + "->" + ex.Message, ex);
        }

        return ValueTask.CompletedTask;
    }
}