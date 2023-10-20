using System.Threading.Tasks;

namespace Luck.UnitTest.Pipeline;

public interface IPipe<TContext> where TContext : class
{
    ValueTask InvokeAsync(TContext context, HandlerDelegate<TContext> next);
}