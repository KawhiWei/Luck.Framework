# Luck.Framework

Luck.Framework 是一组可组合的 .NET 类库，覆盖模块化启动、依赖注入、数据访问、缓存、事件总线、日志与测试基础设施。运行时类库支持 `net6.0`、`net7.0`、`net8.0`、`net9.0` 和 `net10.0`；源生成器面向 `netstandard2.0`，由 Roslyn 5.9 编译器工具集构建。

## 快速开始

典型 Web 应用从模块系统和自动依赖注入开始：

```bash
dotnet add package Luck.AppModule
dotnet add package Luck.AutoDependencyInjection
```

```csharp
using Luck.AppModule;
using Luck.AutoDependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication<AppModule>();

var app = builder.Build();
app.InitializeApplication();
app.Run();

public sealed class AppModule : LuckAppModule
{
}
```

按需要继续安装数据访问、消息、缓存或日志类库。每个模块的 README 都包含包安装命令、实际公开 API、配置示例和依赖关系。

## 模块导航

### 基础与应用

| 类库 | 用途 |
| --- | --- |
| [Luck.Framework](src/framework/Luck.Framework/README.md) | 领域模型、模块契约、事件总线抽象、扩展方法和通用基础设施。 |
| [Luck.AppModule](src/framework/Luck.AppModule/README.md) | 扫描模块、解析依赖并执行服务配置与应用初始化生命周期。 |
| [Luck.AutoDependencyInjection](src/framework/Luck.AutoDependencyInjection/README.md) | 基于标记接口或特性的自动服务注册，以及应用初始化扩展。 |
| [Luck.AspNetCore](src/framework/Luck.AspNetCore/README.md) | ASP.NET Core 请求取消令牌与 JSON 转换辅助类型。 |
| [Luck.DDD.Domain](src/framework/Luck.DDD.Domain/README.md) | 实体、聚合根及 SQL 仓储抽象。 |
| [Luck.Pipeline](src/framework/Luck.Pipeline/README.md) | 可中断的上下文处理管道与委托管道构建器。 |

### 数据访问

| 类库 | 用途 |
| --- | --- |
| [Luck.EntityFrameworkCore](src/framework/Luck.EntityFrameworkCore/README.md) | EF Core `DbContext` 基类、驱动提供程序抽象、JSON 转换和连接拦截器。 |
| [Luck.EntityFrameworkCore.MySQL](src/framework/Luck.EntityFrameworkCore.MySQL/README.md) | 通过与目标框架匹配的 Pomelo 或 Oracle provider 注册 MySQL 驱动。 |
| [Luck.EntityFrameworkCore.PostgreSQL](src/framework/Luck.EntityFrameworkCore.PostgreSQL/README.md) | 通过 Npgsql 注册 PostgreSQL EF Core 驱动。 |
| [Luck.EntityFrameworkCore.MemoryDataBase](src/framework/Luck.EntityFrameworkCore.MemoryDataBase/README.md) | 注册 EF Core InMemory 驱动，适合测试场景。 |
| [Luck.Dapper](src/framework/Luck.Dapper/README.md) | Dapper 驱动抽象及实体、聚合根 SQL 仓储实现。 |
| [Luck.Dapper.ClickHouse](src/framework/Luck.Dapper.ClickHouse/README.md) | ClickHouse 连接配置、模块和 Dapper 驱动。 |
| [Luck.MongoDB](src/framework/Luck.MongoDB/README.md) | MongoDB 上下文选项与 ObjectId 约定支持。 |

### 消息、缓存与可观测性

| 类库 | 用途 |
| --- | --- |
| [Luck.EventBus.RabbitMQ](src/framework/Luck.EventBus.RabbitMQ/README.md) | RabbitMQ 集成事件发布、订阅与后台消费服务。 |
| [Luck.EventBus.OpenTelemetry](src/framework/Luck.EventBus.OpenTelemetry/README.md) | 将 Luck 事件总线诊断事件转换为 OpenTelemetry Activity。 |
| [Luck.EventBus.Kafka](src/framework/Luck.EventBus.Kafka/README.md) | Kafka 连接配置模型；当前不包含生产者或消费者实现。 |
| [Luck.Redis.StackExchange](src/framework/Luck.Redis.StackExchange/README.md) | StackExchange.Redis 连接及 Redis Hash/List 服务注册。 |
| [Luck.Logging.Serilog](src/framework/Luck.Logging.Serilog/README.md) | Serilog 主机日志、滚动文件输出和结构化日志字段。 |

### 工具与测试

| 类库 | 用途 |
| --- | --- |
| [Luck.SourceGenerator](src/framework/Luck.SourceGenerator/README.md) | 用于服务注册的 Roslyn 增量源生成器。 |
| [Luck.TestBase](src/framework/Luck.TestBase/README.md) | 以 Luck 模块和依赖注入容器为基础的集成测试基类。 |

## 常用组合

| 场景 | 建议组合 |
| --- | --- |
| ASP.NET Core 应用 | `Luck.AppModule`、`Luck.AutoDependencyInjection`、`Luck.AspNetCore`、`Luck.Logging.Serilog` |
| EF Core 应用 | `Luck.EntityFrameworkCore` 加一个数据库驱动类库 |
| RabbitMQ 消息处理 | `Luck.EventBus.RabbitMQ`，需要链路追踪时再加 `Luck.EventBus.OpenTelemetry` |
| Dapper + ClickHouse | `Luck.Dapper`、`Luck.Dapper.ClickHouse` |
| 集成测试 | `Luck.TestBase`，按被测模块追加其运行时依赖 |

## 构建与测试

```bash
dotnet restore Luck.sln
dotnet build Luck.sln --no-restore
dotnet test test/Luck.UnitTest/Luck.UnitTest.csproj
```

## 版本记录与许可证

最新变更见 [2.0.14 更新记录](change/2.0.14.md)，历史记录位于 [change](change) 目录。许可证见 [LICENSE](LICENSE)。
