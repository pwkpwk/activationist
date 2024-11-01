using Microsoft.Extensions.Logging;

namespace activationist.app;

public sealed class DependencyOne(ILogger<DependencyOne> logger) : IDependencyOne
{
    private static readonly EventId SaySomethingId = new EventId(1, "SaySomething");

    void IDependencyOne.SaySomething()
    {
        logger.LogInformation(SaySomethingId, "DependencyOne.SaySomething");
    }
}