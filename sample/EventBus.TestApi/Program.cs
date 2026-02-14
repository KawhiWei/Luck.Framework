using EventBus.TestApi;
using Luck.AppModule;
using Luck.AutoDependencyInjection;
using Luck.AutoDependencyInjection.PropertyInjection;
using Luck.EventBus.RabbitMQ;
using Luck.Framework.Event;
using OpenTelemetry.Luck.EventBus;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using EventBus.TestApi.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container using AppWebModule
builder.Services.AddApplication<AppWebModule>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "EventBus Test API",
        Version = "v1",
        Description = "RabbitMQ EventBus 测试 API"
    });
});
builder.Services.AddHttpContextAccessor();

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

// Configure OpenTelemetry with EventBus instrumentation and OTLP/gRPC to OpenObserve
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("EventBus.TestApi", "1.0.0"))
            .AddAspNetCoreInstrumentation()
            .AddLuckEventBusInstrumentation()
            .AddConsoleExporter()
            .AddOtlpExporter(options =>
            {
                // gRPC endpoint for OpenObserve
                options.Endpoint = new Uri(builder.Configuration.GetValue<string>("OTLP:GrpcEndpoint") ?? "http://localhost:5081");
                
                // Use gRPC protocol
                options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                
                // Add headers
                var apiKey = builder.Configuration.GetValue<string>("OTLP:ApiKey") ?? "cm9vdEBleGFtcGxlLmNvbTp3U1J6RDJpSDltR2RWSENs";
                var organization = builder.Configuration.GetValue<string>("OTLP:Organization") ?? "default";
                var streamName = builder.Configuration.GetValue<string>("OTLP:StreamName") ?? "default";
                
                options.Headers = $"Authorization=Basic {apiKey},organization={organization},stream-name={streamName}";
            });
    });

// Register event handlers
builder.Services.AddTransient<IIntegrationEventHandler<TestEvent>, TestEventHandler>();

// Use Property Injection
builder.Host.UsePropertyInjection();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "EventBus Test API v1");
    options.RoutePrefix = string.Empty;
});

app.UseAuthorization();
app.MapControllers();

// Initialize application
app.InitializeApplication();

app.Run();
