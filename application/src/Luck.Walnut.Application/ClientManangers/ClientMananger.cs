using System.Collections.Concurrent;
using Luck.Walnut.Application.Environments;
using Luck.Walnut.Application.Environments.Events;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Luck.Walnut.Application;

public class ClientMananger : IClientMananger  //，NotificationHandler<AppConfigurationEvent>
{
    private readonly IApplactionClientConcurrentQueue _applactionClientConcurrentQueue;

    public ClientMananger(IApplactionClientConcurrentQueue applactionClientConcurrentQueue)
    {
        _applactionClientConcurrentQueue = applactionClientConcurrentQueue;
    }

    private readonly ConcurrentDictionary<string, List<string>> _clients =
        new ConcurrentDictionary<string, List<string>>();

    public void Add(string appId,string connectionId)
    {
        if (_clients.TryGetValue(appId, out var connectionIds))
        {
            var newConnectionIds = connectionIds.Select(x => x).ToList();
            newConnectionIds.Add(connectionId);
            _clients.TryUpdate(appId, newConnectionIds, connectionIds);
        }
        else
        {
            connectionIds = new List<string>()
            {
                connectionId
            };
            _clients.TryAdd(appId, connectionIds);
        }
    }

    public  List<string> GetConnectionIds(string appId)
    {
        if (_clients.TryGetValue(appId, out var connectionIds))
        {
            return connectionIds;
        }
        return new List<string>();
    }
    // public async Task Handle(AppConfigurationEvent notification, CancellationToken cancellationToken)
    // {
    //     await  Task.CompletedTask;
    //     _applactionClientConcurrentQueue.ConcurrentQueue.Enqueue(notification.AppId);
    // }
    
}