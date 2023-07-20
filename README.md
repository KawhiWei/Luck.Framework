# Luck.Framework
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
