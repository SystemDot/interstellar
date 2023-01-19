using MediatR;

namespace Interstellar.Tests.Performance;

public class TestPerformanceEvent : INotification
{
    public Guid Id { get; }

    public TestPerformanceEvent(Guid id)
    {
        Id = id;
    }
}