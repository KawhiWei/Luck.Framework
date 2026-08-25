# Luck.AspNetCore

`Luck.AspNetCore` 为 ASP.NET Core MVC 提供 Luck.Framework 的 Web 集成：统一 API 返回模型、异常与模型验证处理、System.Text.Json 转换器，以及从 HTTP 请求获取取消标记的实现。

## 安装

```bash
dotnet add package Luck.AspNetCore --version 2.0.14
```

该包依赖 `Luck.Framework`，并引用 `Microsoft.AspNetCore.App` shared framework。当前目标框架为 `net6.0`、`net7.0`、`net8.0`、`net9.0` 和 `net10.0`。

## 统一 API 返回

### 注册过滤器

`AddApiResult()` 会关闭 ASP.NET Core 默认的无效模型状态过滤，并把 `IApiResultWrapAttribute` 注册为单例。它不会自动把过滤器加入 MVC 的过滤器集合，需要显式添加：

```csharp
using Luck.AspNetCore.ApiResults;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiResult();
builder.Services.AddControllers(options =>
{
    options.Filters.AddService<IApiResultWrapAttribute>();
});
```

也可以只对指定控制器或 Action 使用 `[ApiResultWrap]`：

```csharp
[ApiController]
[ApiResultWrap]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        return Ok(new { Id = id });
    }
}
```

### 返回结构

`ApiResult` 有四个公开属性：`Success`、`ErrorCode`、`ErrorMessage` 和 `Result`。

成功结果示例：

```json
{
  "success": true,
  "errorCode": null,
  "errorMessage": null,
  "result": { "id": "A001" }
}
```

`ApiResultWrapAttribute` 的实际行为如下：

- 模型状态无效时返回 HTTP 400，错误码为 `Invalid Parameter`，错误信息为模型错误消息拼接结果。
- 成功的 `JsonResult` 或 `ObjectResult` 会把原值放入 `Result`；其他 `IActionResult` 类型取不到值时，`Result` 为 `null`。
- `NotFoundException` 会被清除并设置 HTTP 404，但当前实现不会为这个分支自动生成响应 body。
- `BusinessException` 使用其 `ErrorCode`，记录 Debug 日志；当前实现不会在该分支主动修改 HTTP 状态码。
- 其他异常使用 `Internal Server Error`，设置 HTTP 500 并记录 Error 日志。
- 生产环境的异常响应固定使用“服务器内部错误”；非生产环境返回 `ApiResultWithStackTrace`，其中包含基础异常消息和堆栈。

可通过继承 `ApiResultWrapAttribute` 重写 `OnException(Exception ex)`，在异常转换前替换异常对象。`DisableApiResultWrapAttribute` 可以放在 Action 上跳过成功结果的包装；异常分支仍会按上述规则处理。

## JSON 转换器

`Luck.AspNetCore.Extensions.SystemTextJsonConvert` 提供可选的 `JsonConverter`：

- `DecimalConverter`、`DecimalNullConverter`
- `IntConverter`、`IntNullConverter`
- `BoolConverter`、`BoolNullConverter`
- `DateTimeConverter`、`DateTimeNullConverter`
- `DateTimeOffsetConverter`、`DateTimeOffsetNullConverter`
- `TimeSpanJsonConverter`
- `TimeOnlyJsonConverter`、`TimeOnlyNullJsonConverter`
- `DateOnlyJsonConverter`、`DateOnlyNullJsonConverter`

需要时在 MVC JSON 选项中显式添加：

```csharp
using Luck.AspNetCore.Extensions;

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateOnlyNullJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeConverter());
    options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeNullConverter());
    options.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.TimeOnlyJsonConverter());
});
```

当前格式约定为：`DateTime`/`DateTimeOffset` 写出 `yyyy-MM-dd HH:mm:ss`，`DateOnly` 写出 `yyyy-MM-dd`，`TimeOnly` 和 `TimeSpan` 写出 `HH:mm:ss`。转换器读取字符串时使用当前 .NET 解析规则；使用前应对输入格式和文化设置进行测试。

## HTTP 请求取消标记

`HttpContextCancellationTokenProvider` 实现 `Luck.Framework.Threading.ICancellationTokenProvider`，返回 `HttpContext.RequestAborted`；没有当前 HTTP 请求时回退到 `CancellationToken.None`。需要显式注册：

```csharp
using Luck.AspNetCore;
using Luck.Framework.Threading;

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICancellationTokenProvider, HttpContextCancellationTokenProvider>();
```

服务或控制器中即可通过 `ICancellationTokenProvider.Token` 将请求中断传递给数据库、HTTP 客户端或异步任务。

## 注意事项

- `AddApiResult()` 不会自动加入 MVC 过滤器；使用全局包装时必须调用 `options.Filters.AddService<IApiResultWrapAttribute>()`，或显式标注 `[ApiResultWrap]`。
- 包装器只提取 `JsonResult` 和 `ObjectResult` 的 `Value`。返回文件、重定向或 `NoContentResult` 时请确认是否需要自定义过滤器。
- 非生产环境的统一响应包含异常消息和堆栈，不能把该模式当作生产错误响应格式。
- JSON 转换器不是全局自动注册的；只添加业务真正需要的转换器，避免覆盖项目已有的序列化规则。
