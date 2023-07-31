using Luck.AppModule;
using Luck.AutoDependencyInjection.PropertyInjection;
using Module.Sample;
using System.Diagnostics;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddApplication<AppWebModule>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

//Ìí¼ÓÕâ¸ö¡£
builder.Host.UsePropertyInjection();
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