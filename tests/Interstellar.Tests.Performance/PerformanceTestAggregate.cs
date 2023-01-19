using Interstellar.Configuration;

namespace Interstellar.Tests.Performance;

public class PerformanceTestAggregate : AggregateRoot
{
    public static void Configure(DomainConfiguration configuration)
    {
        configuration
            .ForAggregate<PerformanceTestAggregate>()
            .ReceiveCommand<TestPerformanceCommand>(command => command.Id.ToPerformanceTestStreamId());
    }

    public PerformanceTestAggregate()
    {
        When<TestPerformanceCommand>().Do(command =>
        {
            for (int i = 0; i < command.EventCount; i++)
            {
                Then(new TestPerformanceEvent(command.Id));
            }
        });
    }
}