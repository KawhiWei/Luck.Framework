using System.Collections.Concurrent;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Luck.Walnut.Api.Protos;
using Luck.Walnut.Application.Environments.Events;
using MediatR;

namespace Luck.Walnut.Api.GrpcServices;

[AutoMapGrpcService]
public class LuCatGrpcService : LuCat.LuCatBase, INotificationHandler<AppConfigurationEvent>
{
    private readonly ConcurrentDictionary<string, IServerStreamWriter<BathTheCatResp>> _concurrentQueue =
        new ConcurrentDictionary<string, IServerStreamWriter<BathTheCatResp>>();
    public override async Task BathTheCat(IAsyncStreamReader<BathTheCatReq> requestStream,
        IServerStreamWriter<BathTheCatResp> responseStream, ServerCallContext context)
    {
        while (await requestStream.MoveNext())
        {
            //将要洗澡的猫加入队列
            Console.WriteLine($"Cat {requestStream.Current.Id} Enqueue.");
        }
        _concurrentQueue.TryAdd("aaaa",responseStream);
    }
    public async Task Handle(AppConfigurationEvent notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        Console.WriteLine(notification.Id);
        //if (_concurrentQueue.TryGetValue(out var responseStream))
        //{
          //  await responseStream.WriteAsync(new BathTheCatResp() { Message = $"铲屎的成功给一只{notification.Id}洗了澡！" });
        //}
    }

    public override Task<CountCatResult> Count(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new CountCatResult()
        {
            Count = 80
        });
    }
}