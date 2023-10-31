using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Luck.UnitTest.Pipeline;

public class TestPipeLine
{
    [Fact]
    public async Task New_Pipeline_Test()
    {
        var test = new TrainPaySuccessPipeline();
        var test1 = new TrainPaySuccessGetPipeline();
        var trainPaySuccessContext = new TrainPaySuccessContext()
        {
            Request = new OrderInfo()
            {
                SerialNo = "A00001",
                Type = 2,
            },
            Response = new Response()
            {
                Success = false,
            }
        };
        var pipelineBuilder = PipelineBuilders.CreateBuilder<TrainPaySuccessContext>();
        pipelineBuilder.UsePipe(test);
        pipelineBuilder.UsePipe(test1);
        
        await pipelineBuilder.Build()(trainPaySuccessContext);
        Assert.True(trainPaySuccessContext.Response.Success);
        Assert.True(trainPaySuccessContext.Domain.SerialNo == trainPaySuccessContext.Request.SerialNo);
    }
}