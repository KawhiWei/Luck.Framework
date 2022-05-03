namespace Luck.Walnut.Api.GrpcServices
{
    public static class GrpcEndpointRouteBuilderExtensions1
    {
        public static GrpcServiceEndpointConventionBuilder MapGrpcService(this IEndpointRouteBuilder builder, Type serviceType)
        {
#pragma warning disable CS8602 // 解引用可能出现空引用。
            var method = typeof(GrpcEndpointRouteBuilderExtensions).GetMethod("MapGrpcService", new Type[] { builder.GetType() }).MakeGenericMethod(serviceType);
#pragma warning restore CS8602 // 解引用可能出现空引用。

            ///可以使用表达式目录树

            var grpc = method.Invoke(null, new[] { builder });
#pragma warning disable CS8603 // 可能返回 null 引用。
            return grpc as GrpcServiceEndpointConventionBuilder;
#pragma warning restore CS8603 // 可能返回 null 引用。
        }
    }
}
