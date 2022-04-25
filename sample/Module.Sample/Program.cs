using Luck.Framework.Infrastructure;
using MediatR;
using Module.Sample;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication<AppWebModule>();

builder.Services.AddControllers();

builder.Services.AddMediatR(AssemblyHelper.AllAssemblies);

builder.Services.AddRedis(x =>
{
    x.Timeout = 1000;
    x.Host = "127.0.0.1:6379";

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();


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
app.Run();
