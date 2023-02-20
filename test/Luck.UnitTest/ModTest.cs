using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Luck.UnitTest;

public class ModTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ModTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Amount_ToString_Test()
    {
        var arrAy = new List<int>();
        if (arrAy == null) throw new ArgumentNullException(nameof(arrAy));
        for (var i = 0; i < 3000; i++)
        {
            var mod = i % 100;
            if (mod <= 20)
            {
                arrAy.Add(i);
            }
        }

        _testOutputHelper.WriteLine(string.Join(",", arrAy));
    }
}