using Luck.Framework.Infrastructure;
using MediatR;
using Module.Sample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication<AppWebModule>();

builder.Services.AddControllers();
<<<<<<< HEAD
builder.Services.AddMediatR(AssemblyHelper.AllAssemblies);
=======
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddRedis(x =>
{
    x.Timeout = 1000;
    x.Host = "127.0.0.1:6379";

});
>>>>>>> 23b5e11d2dbbdb471ac868c1dc8ee0b319032187
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
