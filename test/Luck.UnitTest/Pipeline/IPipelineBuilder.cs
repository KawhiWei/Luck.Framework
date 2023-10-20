namespace Luck.UnitTest.Pipeline;

public interface IPipelineBuilder<TContext> where TContext : class
{

    IPipelineBuilder<TContext> UsePipe(IPipe<TContext> pipe);
    
    HandlerDelegate<TContext> Build();
}