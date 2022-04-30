using Luck.Framework.Infrastructure;
using Luck.Walnut.Api.AppModules;
using MediatR;

using Serilog;
using Serilog.Events;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication<AppWebModule>();

builder.Services.AddControllers();
builder.Services.AddMediatR(AssemblyHelper.AllAssemblies);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UsePathBase("/walnut");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();
app.UseRouting();

app.MapControllers();
app.InitializeApplication();
app.Run();
