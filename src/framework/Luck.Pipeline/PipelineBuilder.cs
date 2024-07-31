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

    public IPipeActuator<TContext> Build()
    {
        var defaultActuator = new DefaultPipeActuator<TContext>();
        foreach (var type in _types)
        {
            if (_serviceProvider.GetService(type) is not IPipe<TContext> middleware)
            {
                throw new NotImplementedException("中间件类未注入到DI容器" + type.Name + "请检查.");
            }

            var lastMiddleware = defaultActuator.LastOrDefault();
            if (lastMiddleware is not null)
            {
                lastMiddleware.NextPipe = middleware;
            }

            defaultActuator.Add(middleware);
        }

        return defaultActuator;
    }
}