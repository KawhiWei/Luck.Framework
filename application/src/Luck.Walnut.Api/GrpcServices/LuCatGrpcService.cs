using System.Collections.Concurrent;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Luck.Walnut.Api.Protos;
using Luck.Walnut.Application;
using Luck.Walnut.Application.Environments.Events;
using MediatR;

namespace Luck.Walnut.Api.GrpcServices;

[AutoMapGrpcService]
public class LuCatGrpcService : LuCat.LuCatBase, INotificationHandler<AppConfigurationEvent>
{
    private  readonly  ILogger<LuCatGrpcService> _logger;
    private  readonly  IClientMananger _clientManager;

    public LuCatGrpcService(IClientMananger clientManager,ILogger<LuCatGrpcService> logger)
    {
        _clientManager = clientManager;
        _logger = logger;
    }
    /// <summary>
    /// 发起链接
    /// </summary>
    /// <param name="requestStream"></param>
    /// <param name="responseStream"></param>
    /// <param name="context"></param>
    public override async Task BathTheCat(IAsyncStreamReader<BathTheCatReq> requestStream,
        IServerStreamWriter<BathTheCatResp> responseStream, ServerCallContext context)
    {
        
        // context.
        await responseStream.WriteAsync(new BathTheCatResp() { Message = $"铲屎的成功给一只age洗了澡！" });
        
        
        // Task.Run(async () =>
        // {
        //     while (true)
        //     {
        //         if (_queue.TryTake(out var message))
        //         {
        //             await responseStream.WriteAsync(new BathTheCatResp() { Message = $"铲屎的成功给一只{message}洗了澡！" });
        //         }
        //
        //         await Task.Delay(1000);
        //     }
        // });
        while (await requestStream.MoveNext())
        {
            //将要洗澡的猫加入队列
            Console.WriteLine($"Cat {requestStream.Current.Id} Enqueue.");
        }

        //_clientManager.Update += async (sender, args) => await SendBathTheCatAsync(responseStream, args.Id);

    }


    public async Task Handle(AppConfigurationEvent notification, CancellationToken cancellationToken)
    {
        var list = _clientManager.GetConnectionIds(notification.Id);
        var conn = list.FirstOrDefault(); 
    }
    private async Task SendBathTheCatAsync(IServerStreamWriter<BathTheCatResp> responseStream,string id)
    {
        try
        {
            await responseStream.WriteAsync(new BathTheCatResp() { Message = $"铲屎的成功给一只{id}洗了澡！" });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "BathTheCatAsync");       
        }
    }
    public override Task<CountCatResult> Count(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new CountCatResult()
        {
            Count = 80
        });
    }

}