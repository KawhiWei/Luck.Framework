using System.Diagnostics;
using Luck.Framework.Infrastructure;
using MediatR;
using Module.Sample;
using Luck.AppModule;
using Luck.AutoDependencyInjection;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddApplication<AppWebModule>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
// builder.Services.AddDoveLogger();
builder.Host.UseDefaultPropertyInjection();

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