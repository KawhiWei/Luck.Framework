using System.Collections.Concurrent;

namespace Luck.Walnut.Application.Environments;

public interface IApplactionClientConcurrentQueue:ISingletonDependency
{
    ConcurrentQueue<string> ConcurrentQueue { get; }
}