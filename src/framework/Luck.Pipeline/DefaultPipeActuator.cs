using Luck.Framework.PipelineAbstract;

namespace Luck.Pipeline;

public class DefaultPipeActuator<TContext> : List<IPipe<TContext>>,
    IPipeActuator<TContext> where TContext : IContext
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