using Luck.Framework.PipelineAbstract;

namespace Luck.Pipeline;

public abstract class DefaultDelegatePipe<TContext> : IDelegatePipe<TContext> where TContext : IContext
{
    public async ValueTask InvokeAsync(TContext context, DelegatePipe<TContext> next)
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

            await next(context);
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