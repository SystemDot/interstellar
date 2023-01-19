namespace Interstellar.Tests.Unit;

public class TestStateThreeEvent : IEvent
{
    public Guid Id { get; }
    public int NumberOfEventsPrior { get; }

    public TestStateThreeEvent(Guid id, int numberOfEventsPrior)
    {
        Id = id;
        NumberOfEventsPrior = numberOfEventsPrior;
    }
}