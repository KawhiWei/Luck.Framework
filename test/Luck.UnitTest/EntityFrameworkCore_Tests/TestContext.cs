using System;
using Luck.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Luck.UnitTest.EntityFrameworkCore_Tests;

public class TestContext : LuckDbContextBase
{
    public TestContext(DbContextOptions options, IServiceProvider serviceProvider) : base(options, serviceProvider)
    {
    }
}