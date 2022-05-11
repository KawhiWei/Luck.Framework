using Luck.Framework.Infrastructure;
using MediatR;
using Module.Sample;
using System.Reflection;

using Luck.EventBus.RabbitMQ;
using Luck.Framework.Event;
using Module.Sample.EventHandlers;
using Luck.Framework.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication<AppWebModule>();

builder.Services.AddControllers();

builder.Services.AddMediatR(AssemblyHelper.AllAssemblies);

//builder.Services.AddRedis(x =>
//{
//    x.Timeout = 1000;
//    x.Host = "127.0.0.1:6379";

//});

builder.Services.AddEventBusRabbitMQ(x =>
{
    x.Host = "127.0.0.1";
    x.UserName = "guest";
    x.PassWord = "guest";
    x.Port = 5672;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

//注入处理器  可以使用DependencyInjection 自动注入
//builder.Services.AddTransient(typeof(CreateOrderIntegrationHandler));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.InitializeApplication();
/// <summary>
/// 订阅处理器
/// </summary>
var eventBus = app.Services.GetService<IIntegrationEventBus>();
eventBus?.Subscribe<CreateOrderIntegrationEvent, CreateOrderIntegrationHandler>();


app.Run();
