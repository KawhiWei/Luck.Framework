using System.Threading.Tasks;

namespace Luck.UnitTest.Pipeline;

public class TrainPaySuccessPipeline : PipeBase<TrainPaySuccessContext>
{
    protected override async ValueTask InvokeCoreAsync(TrainPaySuccessContext context)
    {
        await Task.CompletedTask;
        context.Response.Success = true;
    }
}


public class TrainPaySuccessGetPipeline : PipeBase<TrainPaySuccessContext>
{
    protected override async ValueTask InvokeCoreAsync(TrainPaySuccessContext context)
    {
        await Task.CompletedTask;

        context.Domain = new Domain()
        {
            SerialNo = context.Request.SerialNo,
        };
        
        context.Response.Success = true;
    }
}