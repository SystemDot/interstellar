namespace Interstellar.Tests.Unit;

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class AggregateLookupTests
{
    [Fact]
    public void CommandsReceivedByAggregateReportCorrectly()
    {
        ServiceProvider services = ServicesBuilder.BuildServices();
        AggregateLookup aggregateLookup = services.GetService<AggregateLookup>()!;
        aggregateLookup.CommandIsReceivedByAnAggregate<TestStateOneCommand>().Should().BeTrue();
        aggregateLookup.CommandIsReceivedByAnAggregate<TestStateTwoCommand>().Should().BeTrue();
        aggregateLookup.CommandIsReceivedByAnAggregate<TestStateThreeCommand>().Should().BeTrue();
        aggregateLookup.CommandIsReceivedByAnAggregate<UnknownCommand>().Should().BeFalse();
    }
}