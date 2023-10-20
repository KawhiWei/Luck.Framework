using System.Threading.Tasks;

namespace Luck.UnitTest.Pipeline;

public delegate ValueTask HandlerDelegate<in TContext>(TContext context) where TContext : class;