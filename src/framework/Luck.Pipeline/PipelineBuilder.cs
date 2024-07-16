using Luck.Framework.PipelineAbstract;

namespace Luck.Pipeline;

public class PipelineBuilder<TContext> : IPipelineBuilder<TContext> where TContext : IContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<Type> _types = [];

    public PipelineBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPipelineBuilder<TContext> UseMiddleware<TMiddleware>() where TMiddleware : IMiddleware<TContext>
    {
        _types.Add(typeof(TMiddleware));
        return this;
    }

    public IPipe<TContext> Build()
    {
        var line = new DefaultPipeline<TContext>();
        foreach (var type in _types)
        {
            var middleware = _serviceProvider.GetService(type) as IMiddleware<TContext>;
            if (middleware == null)
            {
                throw new NotImplementedException("中间件类" + type.Name + "未实现,请检查.");
            }

            var lastMiddleware = line.LastOrDefault();
            if (lastMiddleware is not null)
            {
                lastMiddleware.NextMiddleware = middleware;
            }

            line.Add(middleware);
        }

        return line;
    }
    
    
}