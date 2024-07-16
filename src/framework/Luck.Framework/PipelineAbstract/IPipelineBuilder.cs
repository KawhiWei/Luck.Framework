namespace Luck.Framework.PipelineAbstract;

public interface IPipelineBuilder<TContext> where TContext : IContext
{
    IPipelineBuilder<TContext> UseMiddleware<TMiddleware>() where TMiddleware : IPipe<TContext>;

    IPipeActuator<TContext> Build();
    
}