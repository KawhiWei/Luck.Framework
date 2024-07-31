using System;
using Luck.EntityFrameworkCore.DbContexts;
using Luck.UnitTest.Models;
using Microsoft.EntityFrameworkCore;

namespace Luck.UnitTest.EntityFrameworkCore_Tests;

public class TestContext : LuckDbContextBase
{
    public TestContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}