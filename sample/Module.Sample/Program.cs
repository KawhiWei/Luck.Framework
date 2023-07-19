using System.Diagnostics;
using Luck.Framework.Infrastructure;
using MediatR;
using Module.Sample;
using Luck.AppModule;



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

// builder.Services.AddEventBusRabbitMq(x =>
// {
//     x.UserName = "kawhi";
//     x.Host = "192.168.31.40";
//     x.PassWord = "wzw0126..";
//     x.Port = 5672; // 40014 管理面板
// });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
// builder.Services.AddDoveLogger();

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