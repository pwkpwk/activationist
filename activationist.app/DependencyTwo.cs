using Microsoft.Extensions.Logging;

namespace activationist.app;

public class DependencyTwo(ILogger<DependencyTwo> logger) : IDependencyTwo
{
    private static readonly EventId DoSomethingId = new EventId(1, "DoSomething");
    
    void IDependencyTwo.DoSomething()
    {
        logger.LogInformation(DoSomethingId, "DependencyTwo.DoSomething");
    }
}