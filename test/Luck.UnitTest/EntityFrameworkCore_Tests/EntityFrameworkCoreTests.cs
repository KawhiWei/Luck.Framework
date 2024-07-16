using System;
using System.Threading.Tasks;
using Luck.DDD.Domain.Repositories;
using Luck.EntityFrameworkCore.DbContextDrivenProvides;
using Luck.Framework.UnitOfWorks;
using Luck.UnitTest.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Luck.UnitTest.EntityFrameworkCore_Tests;

public class EntityFrameworkCoreTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ServiceProvider _serviceProvider;
    private readonly string _connectionString = Guid.NewGuid().ToString();
    private readonly IAggregateRootRepository<Order, string> _orderAggregateRootRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EntityFrameworkCoreTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var services = new ServiceCollection();
        services.AddLuckDbContext<TestContext>(x =>
            {
                x.ConnectionString = _connectionString;
                x.Type = DataBaseType.MemoryDataBase;
            }).AddLogging()
            .AddUnitOfWork()
            .AddDefaultRepository();
        _serviceProvider = services.BuildServiceProvider();
        _unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
        _orderAggregateRootRepository = _serviceProvider.GetRequiredService<IAggregateRootRepository<Order, string>>();
    }

    [Fact]
    public async Task AggregateRootRepositoryAddAsync()
    {
        try
        {
            _orderAggregateRootRepository.Add(new Order("Pual", "Los"));
            await _unitOfWork.CommitAsync();
        }
        catch (Exception e)
        {
            _testOutputHelper.WriteLine(e.ToString());
            throw;
        }
    }
}