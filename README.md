# Luck.Framework
+ ## change 2.0.6 version
    + ### Fixed
      + **1、修复app.InitializeApplication()初始化是报空判断问题**
---
+ ## change 2.0.5 version
    + ### add
        + **1、添加Luck.Pipeline，管道执行器**
    + ### update
        + **1、添加EF注入池化上下文;**
        + **2、优化RabbitMQ消息总线性能;**
---
+ ## change 2.0.4 version 
  + ### add
    + **1、Luck.EntityFrameworkCore.MemoryDataBase**
  + ### update
    + **1、Luck.Framework支持Net7.0和8.0**
    + **2、Luck.DDD.Domain支持Net7.0和8.0**
    + **3、Luck.AspNetCore支持Net7.0和8.0**
    ```c#
        <ItemGroup Condition="$(TargetFramework) == 'net6.0'">
          <FrameworkReference Include="Microsoft.AspNetCore.App" />
        </ItemGroup>
        <ItemGroup Condition="$(TargetFramework) == 'net7.0'">
            <FrameworkReference Include="Microsoft.AspNetCore.App" />
        </ItemGroup>
        <ItemGroup Condition="$(TargetFramework) == 'net8.0'">
            <FrameworkReference Include="Microsoft.AspNetCore.App" />
        </ItemGroup>
      ```
    + **4、Luck.AppModule支持Net7.0和8.0**
    + **5、Luck.AutoDependencyInjection支持Net7.0和8.0；移动模块化扩展方法到Luck.AutoDependencyInjection类库**
    + **6、Luck.MongoDB支持Net7.0和8.0**
    + **7、Luck.EntityFrameworkCore支持EFCore7.0he 8.0**
    + **8、Luck.EntityFrameworkCore.MySQL；升级6.0的nuget包版本**
    + **9、Luck.EntityFrameworkCore.PostGreSQL支持Net7.0和8.0；升级6.0的nuget包版本**
    + **10、Luck.Dapper支持Net7.0和8.0;**
    + **11、Luck.Dapper.ClickHouse支持Net7.0和8.0**
    + **12、Luck.EventBus.RabbitMQ支持Net7.0和8.0；升级Polly和RabbitMQ.Client版本；通过一下代码导入需要Hosting等组件，不在单独引入包**
    ```c#
        <ItemGroup Condition="$(TargetFramework) == 'net6.0'">
          <FrameworkReference Include="Microsoft.AspNetCore.App" />
        </ItemGroup>
        <ItemGroup Condition="$(TargetFramework) == 'net7.0'">
            <FrameworkReference Include="Microsoft.AspNetCore.App" />
        </ItemGroup>
        <ItemGroup Condition="$(TargetFramework) == 'net8.0'">
            <FrameworkReference Include="Microsoft.AspNetCore.App" />
        </ItemGroup>
      ```
    + **13、Luck.Redis.StackExchange支持Net7.0和8.0；升级6.0的nuget包版本**
---
+ ## change 2.0.3 version
  + ### update
    + **1、Luck.Framework内的System.Text.Json自定义序列化删除，迁移到Luck.AspNetCore**
    + 添加System.Text.Json自定义序列化扩展；
    ```c#
        builder.Services.AddControllers()
                .AddJsonOptions(c =>
                {
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.TimeOnlyJsonConverter());
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateOnlyJsonConverter());
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.TimeOnlyNullJsonConverter());
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateOnlyNullJsonConverter());
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeConverter());
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeNullConverter());  
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeOffsetConverter());
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeOffsetNullConverter());
                });
      ```
---
+ ## change 2.0.2 version
  + ### Add
    + **1、Luck.Framework**
      + 添加System.Text.Json自定义序列化扩展；
      ```c#
        builder.Services.AddControllers()
                .AddJsonOptions(c =>
                {
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.TimeOnlyJsonConverter());
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateOnlyJsonConverter());
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.TimeOnlyNullJsonConverter());
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateOnlyNullJsonConverter());
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeConverter());
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeNullConverter());  
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeOffsetConverter());
                c.JsonSerializerOptions.Converters.Add(new SystemTextJsonConvert.DateTimeOffsetNullConverter());
                });
      ```
---
+ ## change 2.0.1 version
  + ### Update
    + **1、Luck.DDD.Domain**
      + Domain层中的基础聚合根和实体修改命名空间；
  + ### Remove
    + **1、Luck.EntityFrameworkCore**
      + 删除EntityFrameworkCore层中使用的MediatR实现的领域事件，由业务控制，不在基础框架内封装；
    + **2、Luck.DDD.Domain**
      + 删除MediatR实现的领域事件通知；
---
+ ## change 2.0.0 version
  + ### Add 
    + **1、Luck.AppModule**
      + Luck.AppModule模块化具体的实现代码，现在将模块化的代码从Luck.Framework中分离出来；
    + **2、Luck.AutoDependencyInjection**
      + 自动注册到DI容器的模块，现在也将自动注册到DI容器内的代码从Luck.Framework中分离出来，
      + 添加属性注入的实现；
  + ### Remove
    + **1、Luck.Framework**
      + 删除Luck.Framework中的模块化实现的代码，Luck.Framework仅保留接口，不在有任何实现；
      + 删除Luck.Framework中的自动注册到DI容器内的代码；
---
+ ## change 1.0.7 version
  + **1、修改Dapper映射特性表名和列名到Luck.Framework类库**
    + 1.1 修改Dapper的LuckTable和LuckColumn特性到Luck.Framework类库
    + 1.2 调整Luck.Redis.StackExchange到解决方案的5.framework内的cache文件夹，
    + 1.3 调整Luck.MongoDB到解决方案的5.framework内的mongodb文件夹，
---
+ ## change 1.0.6 version
  + **1、Luck.Dapper.ClickHouse**
    + 1.1 优化ClickHouse链接字符串实现，支持ClickHouse多主机连接字符串配置
  + **2、Luck.EventBus.RabbitMQ**
    + 2.1 优化RabbitMQ未将消费Handler注册到容器消息丢失问题
  + **3、添加对MongoDB模块的支持** 
---
+ ## change 1.0.5 version
  + **1、Luck.Framework**
    + 1.1 EFCore添加IQueryable和IEnumerable的WhereIf()扩展；
  + **2、Luck.EntityFrameworkCore**
    + 2.1 EFCore添加IQueryable().WhereIf()扩展；
