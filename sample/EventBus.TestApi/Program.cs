using Luck.EventBus.RabbitMQ;
using Luck.Framework.Event;
using OpenTelemetry.Luck.EventBus;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using EventBus.TestApi;
using EventBus.TestApi.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure RabbitMQ EventBus
builder.Services.AddEventBusRabbitMq(config =>
{
    config.Host = builder.Configuration.GetValue<string>("RabbitMQ:Host") ?? "localhost";
    config.Port = builder.Configuration.GetValue<int>("RabbitMQ:Port");
    config.UserName = builder.Configuration.GetValue<string>("RabbitMQ:UserName") ?? "guest";
    config.PassWord = builder.Configuration.GetValue<string>("RabbitMQ:PassWord") ?? "guest";
    config.VirtualHost = builder.Configuration.GetValue<string>("RabbitMQ:VirtualHost") ?? "/";
    config.RetryCount = 5;
});

// Configure OpenTelemetry with EventBus instrumentation
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("EventBus.TestApi", "1.0.0"))
            .AddAspNetCoreInstrumentation()
            .AddLuckEventBusInstrumentation()  // Add EventBus instrumentation
            .AddConsoleExporter();  // Output traces to console
    });

// Register event handlers
builder.Services.AddTransient<IIntegrationEventHandler<TestEvent>, TestEventHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// Subscribe to events
app.UseEventBus(new[] { typeof(TestEvent) });

app.Run();
