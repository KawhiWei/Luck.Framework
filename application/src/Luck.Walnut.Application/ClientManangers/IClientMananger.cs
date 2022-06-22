using System.Collections.Concurrent;
using Luck.Walnut.Application.Environments.Events;

namespace Luck.Walnut.Application;

public interface IClientMananger:ISingletonDependency
{
    void Add(string appId,string connectionId);
    
    List<string> GetConnectionIds(string appId);
}