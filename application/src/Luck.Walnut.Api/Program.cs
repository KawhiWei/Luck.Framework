using System.Net.Security;
using Luck.AspNetCore;
using Luck.Framework.Infrastructure;
using Luck.Framework.Threading;
using Luck.Walnut.Api.AppModules;
using Luck.Walnut.Api.GrpcServices;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Reflection;
using Luck.WebSocket.Server;
using Luck.WebSocket.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(x =>
{
    // x.ListenAnyIP(5094, opt => opt.Protocols = HttpProtocols.Http2);
    x.ListenAnyIP(5099, opt => opt.Protocols = HttpProtocols.Http1);
    x.ListenAnyIP(5264, opt => opt.Protocols = HttpProtocols.Http2);
});


var test = Environment.GetEnvironmentVariable("AppId");
// Add services to the container.
builder.Services.AddApplication<AppWebModule>();

builder.Services.AddControllers();

builder.Services.AddGrpc();

builder.Services.AddWebSocketConfigRouterEndpoint(x =>
{
    x.WebSocketChannels = new Dictionary<string, WebSocketRouteOption.WebSocketChannelHandler>()
    {
        { "/im", new MvcChannelHandler(4 * 1024).ConnectionEntry }
    };
    x.ApplicationServiceCollection = builder.Services;
});

builder.Services.AddMediatR(AssemblyHelper.AllAssemblies);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICancellationTokenProvider, HttpContextCancellationTokenProvider>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UsePathBase("/walnut");

#region WebSocket

var webSocketOptions = new WebSocketOptions()
{
    KeepAliveInterval = TimeSpan.FromSeconds(15), //服务的主动向客户端发起心跳检测时间
    ReceiveBufferSize = 4 * 1024 //数据缓冲区
};
app.UseWebSockets(webSocketOptions);
app.UseWebSocketServer(app.Services);

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseAuthorization();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<GetConfigService>();
    endpoints.MapGrpcService<LuCatGrpcService>();
    
});


app.MapControllers();
app.InitializeApplication();
app.Run();