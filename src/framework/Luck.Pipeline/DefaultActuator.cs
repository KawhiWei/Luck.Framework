using Luck.Framework.PipelineAbstract;

namespace Luck.Pipeline;

public class DefaultActuator<TContext> : List<IPipe<TContext>>,
    IActuator<TContext> where TContext : IContext
{
    public async ValueTask InvokeAsync(TContext context)
    {
        var pipe = this.FirstOrDefault();

        if (pipe != null)
        {
            await pipe.ExecuteAsync(context);
        }
    }
}