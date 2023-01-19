namespace Interstellar.Tests.Performance;

public class TestPerformanceCommand
{
    public Guid Id { get; }
    public int EventCount { get; }

    public TestPerformanceCommand(Guid id, int eventCount)
    {
        Id = id;
        EventCount = eventCount;
    }
}