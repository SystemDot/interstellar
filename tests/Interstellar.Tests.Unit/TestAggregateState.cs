namespace Interstellar.Tests.Unit;

public class TestAggregateState : 
    AggregateState,
    IApplyEventToState<TestStateOneEvent>,
    IApplyEventToState<TestStateTwoEvent>,
    IApplyEventToState<TestStateThreeEvent>
{
    public int NumberOfEventsPrior { get; private set; }

    public void Apply(TestStateOneEvent toApply)
    {
        NumberOfEventsPrior++;
    }

    public void Apply(TestStateTwoEvent toApply)
    {
        NumberOfEventsPrior++;
    }

    public void Apply(TestStateThreeEvent toApply)
    {
        NumberOfEventsPrior++;
    }
}