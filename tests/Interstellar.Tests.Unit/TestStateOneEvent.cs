namespace Interstellar.Tests.Unit;

public class TestStateOneEvent : IEvent
{
    public Guid Id { get; }

    public TestStateOneEvent(Guid id)
    {
        Id = id;
    }
}