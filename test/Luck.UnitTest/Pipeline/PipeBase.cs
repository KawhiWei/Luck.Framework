using System.Threading.Tasks;

namespace Luck.UnitTest.Pipeline;

public abstract class PipeBase<TContext> : IPipe<TContext> where TContext : class
{
    public async ValueTask InvokeAsync(TContext context, HandlerDelegate<TContext> next)
    {
        // 当前核心流程
        var valueTask1 = InvokeCoreAsync(context);
        // 不能直接判断完成，内部抛异常时，不await，外层捕获不到异常
        // if (!valueTask1.IsCompleted)
        // {
        await valueTask1;
        // }

        // 保证调用下一步
        var valueTask2 = next(context);
        await valueTask2;
    }

    protected abstract ValueTask InvokeCoreAsync(TContext context);
}