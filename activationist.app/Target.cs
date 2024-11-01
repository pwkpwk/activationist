using Microsoft.Extensions.Logging;

namespace activationist.app;

public class Target(IDependencyOne one, IDependencyTwo two, ILogger<Target> logger)
{
    private static readonly EventId InvokeId = new(1, "Invoke");
    private static readonly EventId TaggedId = new(2, "Tagged");
    
    public Target(IDependencyOne one, IDependencyTwo two, ILogger<Target> logger, string tag) : this(one, two, logger) 
    {
        logger.LogInformation(TaggedId, $"Tagged constructor: '{tag}'");
    }
    
    public void Invoke() 
    {
        logger.LogInformation(InvokeId, "Invoke");
        one.SaySomething();
        two.DoSomething();
    }
}