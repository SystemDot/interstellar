namespace Interstellar.Tests.Unit;

public class TestStateTwoEvent : IEvent
{
    public Guid Id { get; }

    public TestStateTwoEvent(Guid id)
    {
        Id = id;
    }
}