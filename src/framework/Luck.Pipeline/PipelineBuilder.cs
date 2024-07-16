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

    public IPipelineBuilder<TContext> UseMiddleware<TMiddleware>() where TMiddleware : IPipe<TContext>
    {
        _types.Add(typeof(TMiddleware));
        return this;
    }

    public IActuator<TContext> Build()
    {
        var line = new DefaultActuator<TContext>();
        foreach (var type in _types)
        {
            var middleware = _serviceProvider.GetService(type) as IPipe<TContext>;
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