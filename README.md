# Luck.Framework

Luck.Framework 是一个模块化的 .NET 开发框架，提供了一系列开箱即用的组件和工具，帮助开发者快速构建企业级应用程序。

## 📋 目录

- [项目介绍](#项目介绍)
- [支持的 .NET 版本](#支持的-net-版本)
- [核心模块](#核心模块)
- [快速开始](#快速开始)
- [模块详细说明](#模块详细说明)
- [版本更新记录](#版本更新记录)
- [贡献指南](#贡献指南)
- [许可证](#许可证)

## 🚀 项目介绍

Luck.Framework 是一个基于模块化设计理念的 .NET 开发框架，旨在：

- **模块化设计**：通过 Luck.AppModule 实现应用的模块化组织
- **依赖注入**：提供自动依赖注入和属性注入支持
- **数据访问**：支持多种数据访问技术（EF Core、Dapper、MongoDB）
- **消息队列**：支持 RabbitMQ 和 Kafka 分布式事件总线
- **缓存支持**：集成 Redis 缓存
- **源代码生成**：提供编译时代码生成能力

## 🔧 支持的 .NET 版本

| 版本 | 状态 |
|------|------|
| .NET 6.0 | ✅ 支持 |
| .NET 7.0 | ✅ 支持 |
| .NET 8.0 | ✅ 支持 |
| .NET 9.0 | ✅ 支持 |
| .NET 10.0 | ✅ 支持 (Preview) |

## 📦 核心模块

### 基础模块

| 模块 | 描述 | NuGet |
|------|------|-------|
| **Luck.Framework** | 核心框架，提供基础接口和抽象 | [![NuGet](https://img.shields.io/nuget/v/Luck.Framework.svg)](https://www.nuget.org/packages/Luck.Framework/) |
| **Luck.AppModule** | 应用模块化实现 | [![NuGet](https://img.shields.io/nuget/v/Luck.AppModule.svg)](https://www.nuget.org/packages/Luck.AppModule/) |
| **Luck.AutoDependencyInjection** | 自动依赖注入和属性注入 | [![NuGet](https://img.shields.io/nuget/v/Luck.AutoDependencyInjection.svg)](https://www.nuget.org/packages/Luck.AutoDependencyInjection/) |
| **Luck.AspNetCore** | ASP.NET Core 扩展支持 | [![NuGet](https://img.shields.io/nuget/v/Luck.AspNetCore.svg)](https://www.nuget.org/packages/Luck.AspNetCore/) |

### 数据访问模块

| 模块 | 描述 | NuGet |
|------|------|-------|
| **Luck.EntityFrameworkCore** | Entity Framework Core 基础支持 | [![NuGet](https://img.shields.io/nuget/v/Luck.EntityFrameworkCore.svg)](https://www.nuget.org/packages/Luck.EntityFrameworkCore/) |
| **Luck.EntityFrameworkCore.MySQL** | MySQL 数据库支持 | [![NuGet](https://img.shields.io/nuget/v/Luck.EntityFrameworkCore.MySQL.svg)](https://www.nuget.org/packages/Luck.EntityFrameworkCore.MySQL/) |
| **Luck.EntityFrameworkCore.PostgreSQL** | PostgreSQL 数据库支持 | [![NuGet](https://img.shields.io/nuget/v/Luck.EntityFrameworkCore.PostgreSQL.svg)](https://www.nuget.org/packages/Luck.EntityFrameworkCore.PostgreSQL/) |
| **Luck.EntityFrameworkCore.MemoryDataBase** | 内存数据库支持（测试用） | [![NuGet](https://img.shields.io/nuget/v/Luck.EntityFrameworkCore.MemoryDataBase.svg)](https://www.nuget.org/packages/Luck.EntityFrameworkCore.MemoryDataBase/) |
| **Luck.Dapper** | Dapper ORM 支持 | [![NuGet](https://img.shields.io/nuget/v/Luck.Dapper.svg)](https://www.nuget.org/packages/Luck.Dapper/) |
| **Luck.Dapper.ClickHouse** | ClickHouse 数据库支持 | [![NuGet](https://img.shields.io/nuget/v/Luck.Dapper.ClickHouse.svg)](https://www.nuget.org/packages/Luck.Dapper.ClickHouse/) |
| **Luck.MongoDB** | MongoDB NoSQL 支持 | [![NuGet](https://img.shields.io/nuget/v/Luck.MongoDB.svg)](https://www.nuget.org/packages/Luck.MongoDB/) |

### 消息队列模块

| 模块 | 描述 | NuGet |
|------|------|-------|
| **Luck.EventBus.RabbitMQ** | RabbitMQ 事件总线 | [![NuGet](https://img.shields.io/nuget/v/Luck.EventBus.RabbitMQ.svg)](https://www.nuget.org/packages/Luck.EventBus.RabbitMQ/) |
| **Luck.EventBus.Kafka** | Kafka 事件总线 | [![NuGet](https://img.shields.io/nuget/v/Luck.EventBus.Kafka.svg)](https://www.nuget.org/packages/Luck.EventBus.Kafka/) |

### 缓存模块

| 模块 | 描述 | NuGet |
|------|------|-------|
| **Luck.Redis.StackExchange** | Redis 缓存支持 | [![NuGet](https://img.shields.io/nuget/v/Luck.Redis.StackExchange.svg)](https://www.nuget.org/packages/Luck.Redis.StackExchange/) |

### 其他模块

| 模块 | 描述 | NuGet |
|------|------|-------|
| **Luck.DDD.Domain** | DDD 领域驱动设计基础 | [![NuGet](https://img.shields.io/nuget/v/Luck.DDD.Domain.svg)](https://www.nuget.org/packages/Luck.DDD.Domain/) |
| **Luck.Pipeline** | 管道处理模式支持 | [![NuGet](https://img.shields.io/nuget/v/Luck.Pipeline.svg)](https://www.nuget.org/packages/Luck.Pipeline/) |
| **Luck.SourceGenerator** | 源代码生成器 | [![NuGet](https://img.shields.io/nuget/v/Luck.SourceGenerator.svg)](https://www.nuget.org/packages/Luck.SourceGenerator/) |
| **Luck.TestBase** | 单元测试基础类库 | [![NuGet](https://img.shields.io/nuget/v/Luck.TestBase.svg)](https://www.nuget.org/packages/Luck.TestBase/) |

## 🏁 快速开始

### 1. 安装基础包

```bash
# 安装核心框架
dotnet add package Luck.Framework

# 安装应用模块
dotnet add package Luck.AppModule

# 安装自动依赖注入
dotnet add package Luck.AutoDependencyInjection

# 安装 ASP.NET Core 支持
dotnet add package Luck.AspNetCore
```

### 2. 配置 Startup

```csharp
using Luck.AppModule;
using Luck.AutoDependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// 添加 Luck 框架服务
builder.Services.AddApplication<YourAppModule>();

var app = builder.Build();

// 初始化应用模块
app.InitializeApplication();

app.Run();
```

### 3. 创建应用模块

```csharp
using Luck.AppModule;

public class YourAppModule : AppModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 配置服务
        context.Services.AddControllers();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        // 应用初始化
        var app = context.GetApplicationBuilder();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
```

### 4. 自动依赖注入示例

```csharp
// 定义服务接口
public interface IUserService
{
    Task<User> GetByIdAsync(int id);
}

// 实现服务并添加自动注入特性
[Dependency(ServiceLifetime.Scoped)]
public class UserService : IUserService
{
    // 属性注入
    [Autowired]
    public ILogger<UserService> Logger { get; set; }

    public Task<User> GetByIdAsync(int id)
    {
        // 实现逻辑
    }
}
```

## 📖 模块详细说明

### Luck.AppModule - 应用模块化

应用模块化是 Luck.Framework 的核心设计理念。通过模块化，可以将应用拆分为独立的、可复用的模块。

```csharp
// 定义模块
public class DataModule : AppModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(context.Configuration.GetConnectionString("Default"));
        });
    }
}

// 主模块依赖其他模块
[DependsOn(typeof(DataModule))]
public class MainModule : AppModule
{
    // ...
}
```

### Luck.AutoDependencyInjection - 自动依赖注入

支持接口注入和属性注入，简化依赖注册过程。

**特性说明：**
- `[Dependency]` - 标记服务自动注册到 DI 容器
- `[Autowired]` - 标记属性自动注入
- `[Transient]` / `[Scoped]` / `[Singleton]` - 指定生命周期

### Luck.EntityFrameworkCore - 数据访问

提供 EF Core 的集成支持，包含工作单元、仓储模式等。

```csharp
// 配置 EF Core
services.AddLuckDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("Default"));
});
```

### Luck.EventBus - 事件总线

支持 RabbitMQ 和 Kafka 两种消息队列。

```csharp
// RabbitMQ 配置
services.AddRabbitMQEventBus(options =>
{
    options.HostName = "localhost";
    options.UserName = "guest";
    options.Password = "guest";
});

// 发布事件
await eventBus.PublishAsync(new OrderCreatedEvent { OrderId = 1 });

// 订阅事件
public class OrderCreatedHandler : IEventHandler<OrderCreatedEvent>
{
    public Task HandleAsync(OrderCreatedEvent eventData)
    {
        // 处理逻辑
    }
}
```

### Luck.Redis.StackExchange - 缓存

提供 Redis 缓存的封装。

```csharp
// 配置 Redis
services.AddLuckRedis(options =>
{
    options.ConnectionString = "localhost:6379";
});

// 使用缓存
public class MyService
{
    private readonly IRedisCache _cache;

    public MyService(IRedisCache cache)
    {
        _cache = cache;
    }

    public async Task<string> GetDataAsync()
    {
        return await _cache.GetOrCreateAsync("key", async () =>
        {
            return await FetchDataFromDbAsync();
        }, TimeSpan.FromMinutes(5));
    }
}
```

## 📝 版本更新记录

所有版本更新记录请查看 `change/` 目录：

### 最新版本 (2.0.8)

- ✅ 添加对 .NET 9.0 和 .NET 10.0 的支持
- ✅ 升级所有依赖包到最新版本
- ✅ StackExchange.Redis: 2.8.0 → 2.11.0
- ✅ Dapper: 2.1.35 → 2.1.66
- ✅ Confluent.Kafka: 2.5.0 → 2.13.0

### 历史版本

| 版本 | 文件 |
|------|------|
| 2.0.8 | [change/2.0.8.md](change/2.0.8.md) |
| 2.0.6 | [change/2.0.6.md](change/2.0.6.md) |
| 2.0.5 | [change/2.0.5.md](change/2.0.5.md) |
| 2.0.4 | [change/2.0.4.md](change/2.0.4.md) |
| 2.0.3 | [change/2.0.3.md](change/2.0.3.md) |
| 2.0.2 | [change/2.0.2.md](change/2.0.2.md) |
| 2.0.1 | [change/2.0.1.md](change/2.0.1.md) |
| 2.0.0 | [change/2.0.0.md](change/2.0.0.md) |
| 1.0.7 | [change/1.0.7.md](change/1.0.7.md) |
| 1.0.6 | [change/1.0.6.md](change/1.0.6.md) |
| 1.0.5 | [change/1.0.5.md](change/1.0.5.md) |

## 🤝 贡献指南

欢迎提交 Issue 和 Pull Request 来帮助改进 Luck.Framework！

1. Fork 本仓库
2. 创建您的特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交您的更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打开一个 Pull Request

## 📄 许可证

本项目采用 [LGPL-3.0-only](LICENSE) 许可证开源。

---

**维护者**: [KawhiWei](https://github.com/KawhiWei)  
**项目地址**: [https://github.com/KawhiWei/Luck.Framework](https://github.com/KawhiWei/Luck.Framework)
