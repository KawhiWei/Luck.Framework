using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luck.UnitTest.Pipeline;

public class DefaultPipelineBuilder<TContext> : IPipelineBuilder<TContext> where TContext : class
{
    private  IList<PipeDelegate<TContext>> _pipes = new List<PipeDelegate<TContext>>();
    
    public IPipelineBuilder<TContext> UsePipe(IPipe<TContext> pipe)
    {
        // 将pipe包装成获取责任链的回调
        PipeDelegate<TContext> pipeDelegate = next => context => pipe.InvokeAsync(context, next);
        if (pipeDelegate == null)
        {
            throw new ArgumentNullException(nameof(pipeDelegate));
        }
        _pipes.Add(pipeDelegate);

        return this;
    }

    /// <summary>
    /// <see cref="IPipelineBuilder{TContext}.Build"/>
    /// </summary>
    /// <returns></returns>
    public HandlerDelegate<TContext> Build()
    {
        // 默认最后一个是个空执行
        HandlerDelegate<TContext> next = _ => ValueTask.CompletedTask;

        // 构建责任链，从尾部向前构建
        for (var i = _pipes.Count - 1; i >= 0; i--)
        {
            next = _pipes[i].Invoke(next);
        }

        // 返回责任链第一个pipe
        return next;
    }
}