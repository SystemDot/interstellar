namespace Interstellar.Tests.Unit;

using FluentAssertions;
using Xunit;

public class MessageNameTypeLookupTests
{
    private readonly MessageNameTypeLookup lookup;

    public MessageNameTypeLookupTests()
    {
        lookup =
            UseMessageTypes
                .ThatImplement<IEvent>()
                .FromAssemblyContaining<TestStateOneEvent>()
                .Build();
    }

    [Fact]
    public void MessageNameTypeLookupFillsWithCorrectEventTypes()
    {
        lookup.Lookup(nameof(TestStateOneEvent)).Should().Be(typeof(TestStateOneEvent));
        lookup.Lookup(nameof(TestStateTwoEvent)).Should().Be(typeof(TestStateTwoEvent));
        lookup.Lookup(nameof(TestStateThreeEvent)).Should().Be(typeof(TestStateThreeEvent));
        lookup.Lookup(nameof(TestStateThreeOtherEvent)).Should().Be(typeof(TestStateThreeOtherEvent));
    }

    [Fact]
    public void MessageNameTypeLookupDoesNotFillsWithUnexpectedTypes()
    {
        Exception actualException = null!;

        try
        {
            lookup.Lookup(nameof(UnknownCommand));
        }
        catch (Exception exception)
        {
            actualException = exception;
        }

        actualException.Should().BeOfType<CannotLookupMessageTypeByNameException>();
    }
}