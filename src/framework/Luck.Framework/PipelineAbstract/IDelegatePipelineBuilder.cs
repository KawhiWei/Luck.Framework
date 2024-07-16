namespace Luck.Framework.PipelineAbstract;

public interface IDelegatePipelineBuilder<TContext> where TContext : IContext
{
    // IDelegatePipelineBuilder<TContext> UseMiddleware<TMiddleware>(IDelegatePipe<TContext> delegatePipe) where TMiddleware;

    IDelegatePipelineBuilder<TContext> UsePipe(IDelegatePipe<TContext> delegatePipe);
    DelegatePipe<TContext> Build();
}