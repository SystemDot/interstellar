namespace Interstellar.Tests.Unit;

public class TestStateThreeOtherEvent : IEvent
{
    public Guid Id { get; }
    public int NumberOfEventsPrior { get; }

    public TestStateThreeOtherEvent(Guid id, int numberOfEventsPrior)
    {
        Id = id;
        NumberOfEventsPrior = numberOfEventsPrior;
    }
}