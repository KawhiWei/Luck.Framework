using Luck.Framework.PipelineAbstract;
using Microsoft.Extensions.DependencyInjection;

namespace Luck.Pipeline;

public class DelegatePipelineBuilder<TContext> : IDelegatePipelineBuilder<TContext> where TContext : IContext
{
    private readonly IServiceProvider _serviceProvider;
    private IList<PipelineDelegate<TContext>> _pipes = new List<PipelineDelegate<TContext>>();

    public DelegatePipelineBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IDelegatePipelineBuilder<TContext> UsePipe(IDelegatePipe<TContext> delegatePipe)
    {
        _pipes.Add(PipelineDelegate);
        return this;

        DelegatePipe<TContext> PipelineDelegate(DelegatePipe<TContext> next) =>
            context => delegatePipe.InvokeAsync(context, next);
    }

    public DelegatePipe<TContext> Build()
    {
        DelegatePipe<TContext> next = _ => ValueTask.CompletedTask;

        for (var i = _pipes.Count - 1; i >= 0; i--)
        {
            next = _pipes[i].Invoke(next);
        }

        return next;
    }
}