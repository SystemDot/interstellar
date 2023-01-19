namespace Interstellar.Tests.Unit;

using Interstellar.Configuration;

public class TestAggregate : AggregateRoot<TestAggregateState>
{
    public static void Configure(DomainConfiguration configuration)
    {
        configuration
            .ForAggregate<TestAggregate, TestAggregateState>()
            .ReceiveCommand<TestStateOneCommand>(c => c.Id.ToString())
            .ReceiveCommand<TestStateTwoCommand>(c => c.Id.ToString())
            .ReceiveCommand<TestStateThreeCommand>(c => c.Id.ToString());
    }

    public TestAggregate()
    {
        On<TestStateOneEvent>().Become(StateTwo);
        On<TestStateTwoEvent>().Become(StateThree);
        StateOne();
    }

    private void StateOne()
    {
        When<TestStateOneCommand>().Then(command => new TestStateOneEvent(command.Id));
    }

    private void StateTwo()
    {
        When<TestStateTwoCommand>().Then(command => new TestStateTwoEvent(command.Id));
    }

    private void StateThree()
    {
        When<TestStateThreeCommand>().Do(command =>
        {
            Then(new TestStateThreeEvent(command.Id, State.NumberOfEventsPrior));
            Then(new TestStateThreeOtherEvent(command.Id, State.NumberOfEventsPrior));
        });
    }
}