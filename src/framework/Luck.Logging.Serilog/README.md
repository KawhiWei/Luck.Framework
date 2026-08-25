# Luck.Logging.Serilog

`Luck.Logging.Serilog` 为 ASP.NET Core/Generic Host 提供统一的 Serilog 配置：同时写入控制台和滚动文件，并补齐模块、分类等结构化字段。

## 安装

```bash
dotnet add package Luck.Logging.Serilog --version 2.0.14
```

项目通过 `Microsoft.AspNetCore.App` framework reference 使用 ASP.NET Core，支持 `net6.0`、`net7.0`、`net8.0`、`net9.0` 和 `net10.0`。net6/net7 使用 Serilog.AspNetCore 7.0.0，net8-net10 使用 10.0.0；Serilog.Sinks.File 使用 7.0.0。

## ASP.NET Core 快速开始

在创建应用后调用 `AddLuckSerilog()`：

```csharp
using Luck.Logging.Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.AddLuckSerilog();
builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();
```

`AddLuckSerilog(WebApplicationBuilder)` 会先创建 bootstrap logger，再通过 `AddSerilog` 注册应用日志。没有 `WebApplicationBuilder` 时，可直接向服务集合注册：

```csharp
services.AddLuckSerilog(configuration, environment);
```

发生无法恢复的启动异常时可记录并在 finally 中刷新：

```csharp
try
{
    app.Run();
}
catch (Exception exception)
{
    LoggingExtensions.LogStartupFailure(exception);
    throw;
}
finally
{
    LoggingExtensions.CloseAndFlush();
}
```

## 配置

配置节名称为 `LuckLogging`：

```json
{
  "LuckLogging": {
    "Module": "Orders.Api",
    "FilePath": "logs/orders-.log",
    "MinimumLevel": "Information",
    "MinimumLevelOverrides": {
      "Microsoft.AspNetCore": "Warning",
      "Orders.Api.Infrastructure": "Debug"
    },
    "FileSizeLimitBytes": 104857600,
    "RetainedFileCountLimit": 30,
    "RollOnFileSizeLimit": true,
    "Shared": true,
    "FlushIntervalSeconds": 1
  }
}
```

配置解析规则：

- `AppKey` 环境变量优先于 `LuckLogging:Module`；若两者均为空，则使用入口程序集名称，再为空时使用 `unknown`。
- `FilePath` 支持绝对路径和相对于 content root 的路径。未配置时，默认写入 `<content-root>/logs/<module>-.log`。
- 无效或非正数的文件大小、保留数量和刷新间隔会回退到默认值：100 MiB、30 个文件和 1 秒。
- 日志最低级别默认是 `Information`；无法解析的级别也回退到 `Information`。

## 输出和结构化字段

默认输出模板为：

```text
[时间][级别][模块][分类][子分类][RequestTraceId][Filter1][Filter2][消息]
```

`RequiredLogPropertiesEnricher` 会为缺失字段补充默认值：

| 字段 | 含义 |
| --- | --- |
| `Module` | 配置或 `AppKey` 推导出的模块名 |
| `Category` | 日志分类，默认 `-` |
| `Subcategory` | 子分类，缺失时尝试取 `SourceContext` 的最后一段 |
| `RequestTraceId` | 请求或业务代码提供的追踪标识，默认 `-` |
| `Filter1` | 业务代码可选的筛选字段，默认 `-` |
| `Filter2` | 业务代码可选的筛选字段，默认 `-` |

## 注意事项

- 文件日志目录会在 logger 创建时自动创建。应用进程需要具备目标目录的写权限。
- `CloseAndFlush()` 会关闭并刷新全局 `Serilog.Log`；测试和宿主退出时应避免在仍需写日志的代码之前调用。
- 此包只负责 Serilog 配置，不会替换 ASP.NET Core 的异常处理中间件，也不会自动注册 `UseExceptionHandler`。

## 许可证

本项目采用 [LGPL-3.0-only](../../../LICENSE) 许可证。
