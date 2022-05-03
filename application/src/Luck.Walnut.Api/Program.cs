using Luck.AspNetCore;
using Luck.Framework.Infrastructure;
using Luck.Framework.Threading;
using Luck.Walnut.Api.AppModules;
using Luck.Walnut.Api.GrpcServices;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(x =>
{
    x.ListenAnyIP(5000, opt => opt.Protocols = HttpProtocols.Http2);
    x.ListenAnyIP(5099, opt => opt.Protocols = HttpProtocols.Http1);
});

// Add services to the container.
builder.Services.AddApplication<AppWebModule>();

builder.Services.AddControllers();

builder.Services.AddGrpc();

builder.Services.AddMediatR(AssemblyHelper.AllAssemblies);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICancellationTokenProvider, HttpContextCancellationTokenProvider>();
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

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<GetConfigService>();

    var grpcSevviceTypes = AssemblyHelper.FindTypesByAttribute<AutoMapGrpcServiceAttribute>(); 
    foreach (var item in grpcSevviceTypes)
    {
        endpoints.MapGrpcService(item);
    }

});

app.MapControllers();
app.InitializeApplication();
app.Run();
