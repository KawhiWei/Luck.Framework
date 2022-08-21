using System.Diagnostics;
using Luck.Framework.Infrastructure;
using MediatR;
using Module.Sample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDoveLogger();
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

var diagnosticSourceSubscriber = new DiagnosticSourceSubscriber(new LuckDiagnosticSourceListener());

DiagnosticListener.AllListeners.Subscribe(diagnosticSourceSubscriber);


app.UseAuthorization();

app.MapControllers();
//app.UseEventBusRabbitMQ();
app.InitializeApplication();
app.Run();