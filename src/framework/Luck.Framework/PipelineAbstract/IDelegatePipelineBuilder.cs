namespace Luck.Framework.PipelineAbstract;

public interface IDelegatePipelineBuilder<TContext> where TContext : IContext
{
    IDelegatePipelineBuilder<TContext> UsePipe(IDelegatePipe<TContext> delegatePipe);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    DelegatePipe<TContext> Build();
}