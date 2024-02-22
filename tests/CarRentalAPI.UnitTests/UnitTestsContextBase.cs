
namespace CarRentalAPI.UnitTests;

public abstract class UnitTestsContextBase : IAsyncLifetime
{
    public virtual Task DisposeAsync() => Task.CompletedTask;

    public virtual Task InitializeAsync() => Task.CompletedTask;
}
