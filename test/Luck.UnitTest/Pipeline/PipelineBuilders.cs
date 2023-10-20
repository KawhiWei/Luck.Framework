namespace Luck.UnitTest.Pipeline;

public class PipelineBuilders
{
    public static IPipelineBuilder<TContext> CreateBuilder<TContext>() where TContext : class
        => new DefaultPipelineBuilder<TContext>();
}