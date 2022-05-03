namespace Luck.Walnut.Api.GrpcServices
{

    /// <summary>
    /// 自动注入Grpc服务
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoMapGrpcServiceAttribute: Attribute
    {
    }
}
