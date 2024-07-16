namespace Luck.Framework.PipelineAbstract;

public delegate DelegatePipe<TContext> PipelineDelegate<TContext>(DelegatePipe<TContext> next)
    where TContext : IContext;