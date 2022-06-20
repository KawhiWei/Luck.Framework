using System.Collections.Concurrent;
using Luck.Walnut.Application.Environments.Events;
using MediatR;

namespace Luck.Walnut.Application;

public class ClientMananger : IClientMananger
{
    public ClientMananger()
    {
        Watching();
    }

    private readonly ConcurrentDictionary<string, List<string>> _clients =
        new ConcurrentDictionary<string, List<string>>();
    public event EventHandler<AppConfigurationEvent>? Update;

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
    private void Watching()
    {
        Task.Factory.StartNew(() =>
        {
            while (true)
            {
                // if (_queue.TryTake(out var message))
                // {
                //     Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}------{message}");
                //     //await responseStream.WriteAsync(new BathTheCatResp() { Message = $"铲屎的成功给一只{message}洗了澡！" });                    
                // }

                Thread.Sleep(1000);
                // Task.Delay(1000);
            }
        });
    }

    
}