namespace Luck.Framework.PipelineAbstract;

public interface IPipeActuator<in TContext> where TContext : IContext
{
    ValueTask InvokeAsync(TContext context);
}