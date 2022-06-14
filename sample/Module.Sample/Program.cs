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

builder.Services.AddEventBusRabbitMq(x =>
{
    x.UserName = "admin";
    x.Host = "101.34.26.221";
    x.PassWord = "&duyu789";
    x.Port = 40013; // 40014 管理面板
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
//ע�봦����  ����ʹ��DependencyInjection �Զ�ע��
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
app.UseEventBusRabbitMQ();
app.InitializeApplication();
app.Run();
