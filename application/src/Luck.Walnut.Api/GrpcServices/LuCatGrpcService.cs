using System.Collections.Concurrent;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Luck.Walnut.Api.Protos;
using Luck.Walnut.Application;
using Luck.Walnut.Application.Environments;
using Luck.Walnut.Application.Environments.Events;
using MediatR;

namespace Luck.Walnut.Api.GrpcServices;

[AutoMapGrpcService]
public class LuCatGrpcService : LuCat.LuCatBase
{
    private readonly ILogger<LuCatGrpcService> _logger;
    private readonly IClientMananger _clientManager;
    private readonly IApplactionClientConcurrentQueue _applactionClientConcurrentQueue;

    public LuCatGrpcService(IClientMananger clientManager,IApplactionClientConcurrentQueue applactionClientConcurrentQueue, ILogger<LuCatGrpcService> logger)
    {
        _clientManager = clientManager;
        _applactionClientConcurrentQueue = applactionClientConcurrentQueue;
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
        while (await requestStream.MoveNext())
        {
            //将要洗澡的猫加入队列
            Console.WriteLine($"Cat {requestStream.Current.Id} Enqueue.");
        }
        while (true)
        {
            if (_applactionClientConcurrentQueue.ConcurrentQueue.TryDequeue(out var str))
            {
                await responseStream.WriteAsync(new BathTheCatResp() { Message = $"铲屎的成功给一只{str}洗了澡！" });
            }
            await Task.Delay(1000);
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