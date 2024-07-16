namespace Luck.Framework.PipelineAbstract;

public delegate ValueTask DelegatePipe<in TContext>(TContext context) where TContext : IContext;