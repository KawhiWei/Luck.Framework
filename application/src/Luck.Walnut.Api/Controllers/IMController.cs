using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Luck.WebSocket.Server;
using Luck.WebSocket.Server.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Luck.Walnut.Api.Controllers
{
    [Route("api/[controller]")]
    public class IMController : ControllerBase, IWebSocketSession
    {
        public HttpContext WebSocketHttpContext { get ; set; }
        public System.Net.WebSockets.WebSocket WebSocketClient { get ; set ; }

        public readonly ConcurrentDictionary<string, List<string>> clientConnectionDictionary =
            new ConcurrentDictionary<string, List<string>>();

        [WebSocket]
        [HttpPost]
        public Task<string> ClientLogin(string appId)
        {
            if (clientConnectionDictionary.TryGetValue(appId, out var connectionIds))
            {
                var newConnectionIds = connectionIds.Select(x => x).ToList();
                newConnectionIds.Add(WebSocketHttpContext.Connection.Id);
                clientConnectionDictionary.TryUpdate(appId, newConnectionIds, connectionIds);
            }
            else
            {
                connectionIds = new List<string>()
                {
                    WebSocketHttpContext.Connection.Id
                };
                clientConnectionDictionary.TryAdd(appId, connectionIds);
                MvcChannelHandler.Clients.TryGetValue(WebSocketHttpContext.Connection.Id, out var requestFriendSocket);
                // if (requestFriendSocket != null)
                // {
                //     await requestFriendSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("{\"type\":\"login\",\"data\":\"" + WebSocketHttpContext.Connection.Id + "\"}")), WebSocketMessageType.Text, true, CancellationToken.None);
                // }
            }
            // Console.WriteLine($"12132as1d32sa1d3as1d32{appId}---------->{WebSocketHttpContext.Connection.Id}");
            return Task.FromResult("ok"); //此处返回的数据会自动发送给WebSocket客户端，无需手动发送
        }
    }
}

