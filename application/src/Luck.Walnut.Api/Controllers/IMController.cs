using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Luck.Walnut.Application;
using Luck.Walnut.Application.Environments.Events;
using Luck.WebSocket.Server;
using Luck.WebSocket.Server.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Luck.Walnut.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class IMController : ControllerBase, IWebSocketSession, INotificationHandler<AppConfigurationEvent>
    {
        public HttpContext WebSocketHttpContext { get; set; }
        public System.Net.WebSockets.WebSocket WebSocketClient { get; set; }

        public readonly ConcurrentDictionary<string, List<string>> clientConnectionDictionary =
            new ConcurrentDictionary<string, List<string>>();

        private readonly IClientMananger _clientManager;

        public IMController(IClientMananger clientManager)
        {
            _clientManager = clientManager;
        }

        [WebSocket]
        [HttpPost]
        public Task<string> ClientLogin(string appId)
        {
            _clientManager.Add(appId, WebSocketHttpContext.Connection.Id);
            // if (clientConnectionDictionary.TryGetValue(appId, out var connectionIds))
            // {
            //     var newConnectionIds = connectionIds.Select(x => x).ToList();
            //     newConnectionIds.Add(WebSocketHttpContext.Connection.Id);
            //     clientConnectionDictionary.TryUpdate(appId, newConnectionIds, connectionIds);
            // }
            // else
            // {
            //     connectionIds = new List<string>()
            //     {
            //         WebSocketHttpContext.Connection.Id
            //     };
            //     clientConnectionDictionary.TryAdd(appId, connectionIds);
            //     MvcChannelHandler.Clients.TryGetValue(WebSocketHttpContext.Connection.Id, out var requestFriendSocket);
            //     // if (requestFriendSocket != null)
            //     // {
            //     //     await requestFriendSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes("{\"type\":\"login\",\"data\":\"" + WebSocketHttpContext.Connection.Id + "\"}")), WebSocketMessageType.Text, true, CancellationToken.None);
            //     // }
            // }
            // // Console.WriteLine($"12132as1d32sa1d3as1d32{appId}---------->{WebSocketHttpContext.Connection.Id}");
            return Task.FromResult("ok"); //此处返回的数据会自动发送给WebSocket客户端，无需手动发送
        }

        [NonAction]
        public async Task Handle(AppConfigurationEvent notification, CancellationToken cancellationToken)
        {
            var list = _clientManager.GetConnectionIds(notification.Id);
            var conn = list.FirstOrDefault();
            if (conn is not null)
            {
                MvcChannelHandler.Clients.TryGetValue(conn, out var requestFriendSocket);
                var response = new MvcResponseScheme()
                {
                    RequestTime = DateTime.Now.Ticks,
                    Status = 0,
                    Body = "reload"
                }.Serialize();
                var bytes=new ArraySegment<byte>(Encoding.UTF8.GetBytes(response));
                if(requestFriendSocket is not null)
                {
                    await requestFriendSocket.SendAsync(bytes,WebSocketMessageType.Text, true, CancellationToken.None);
                }
                
            }
        }
    }
}