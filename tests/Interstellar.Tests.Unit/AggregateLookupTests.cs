namespace Interstellar.Tests.Unit;

using FluentAssertions;
using Xunit;

public class AggregateLookupTests
{
    [Fact]
    public void CommandsReceivedByAggregateReportCorrectly()
    {
        ServicesBuilder.BuildServices();
        AggregateLookup aggregateLookup = AggregateLookupContext.Current;
        aggregateLookup.CommandIsReceivedByAnAggregate<TestStateOneCommand>().Should().BeTrue();
        aggregateLookup.CommandIsReceivedByAnAggregate<TestStateTwoCommand>().Should().BeTrue();
        aggregateLookup.CommandIsReceivedByAnAggregate<TestStateThreeCommand>().Should().BeTrue();
        aggregateLookup.CommandIsReceivedByAnAggregate<UnknownCommand>().Should().BeFalse();
    }
}