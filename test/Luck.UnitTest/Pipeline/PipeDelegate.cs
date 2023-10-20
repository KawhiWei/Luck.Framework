namespace Luck.UnitTest.Pipeline;

public delegate HandlerDelegate<TContext> PipeDelegate<TContext>(HandlerDelegate<TContext> next) where TContext : class;