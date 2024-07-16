namespace Luck.Framework.PipelineAbstract;

public interface IActuator<in TContext> where TContext : IContext
{
    ValueTask InvokeAsync(TContext context);
}