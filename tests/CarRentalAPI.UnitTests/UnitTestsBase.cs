using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalAPI.UnitTests;

public class UnitTestsBase<T> : IAsyncLifetime where T : IAsyncLifetime, new()
{
    protected T Context { get; set; } = new();

    public Task DisposeAsync() => Context.DisposeAsync();

    public Task InitializeAsync() => Context.InitializeAsync();
}
