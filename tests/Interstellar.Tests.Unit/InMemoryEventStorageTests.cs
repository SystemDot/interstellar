namespace Interstellar.Tests.Unit
{
    using System;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class InMemoryEventStorageTests
    {
        private readonly ServiceProvider services;

        public InMemoryEventStorageTests()
        {
            services = ServicesBuilder.BuildServices();
        }
        
        [Fact]
        public async Task DeliveringFirstStateTestCommandResultsInEventViaAggregate()
        {
            var command = new TestStateOneCommand(Guid.NewGuid());
            var headers = new Dictionary<string, object>
            {
                { "Header1", "Header1Value" },
                { "Header2", "Header2Value" }
            };

            await services.GetService<DomainCommandDeliverer>()!.DeliverCommandAsync(command, headers);

            EventPayload eventPayload = services.GetService<DeliveredEventContext>()!.Events.Single();
            eventPayload.Id.Should().NotBe(Guid.Empty);
            eventPayload.StreamId.Should().Be(command.Id.ToString());
            eventPayload.CreatedOn.Should().BeWithin(TimeSpan.FromMinutes(10));
            eventPayload.Headers.Should().Equal(headers);
            eventPayload.EventIndex.Should().Be(0);
            eventPayload.EventBody.Should().BeOfType<TestStateOneEvent>();
            eventPayload.EventBody.As<TestStateOneEvent>().Id.Should().Be(command.Id.ToString());
        }

        [Fact]
        public async Task DeliveringTestCommandTwiceDoesNotProduceEventAsItGetsIgnoreThroughStateLocksInAggregate()
        {
            var command = new TestStateOneCommand(Guid.NewGuid());
            var headers = new Dictionary<string, object>();
            
            var domainCommandDeliverer = services.GetService<DomainCommandDeliverer>()!;

            await domainCommandDeliverer.DeliverCommandAsync(command, headers);

            Exception actualException = null!;

            try
            {
                await domainCommandDeliverer.DeliverCommandAsync(command, headers);
            }
            catch (Exception exception)
            {
                actualException = exception;
            }

            actualException.Should().NotBeNull();
            actualException.Should().BeOfType<CommandNotHandledByAggregateException<TestStateOneCommand, TestAggregateState>>();
            services.GetService<DeliveredEventContext>()!.Events.Count.Should().Be(1);
        }

        [Fact]
        public async Task DeliveringSecondStateTestCommandResultsInEventViaAggregateWhenFirstStateCommandIsDeliveredFirst()
        {
            var firstStateCommand = new TestStateOneCommand(Guid.NewGuid());
            var secondStateCommand = new TestStateTwoCommand(firstStateCommand.Id);
            var headers = new Dictionary<string, object>();

            var domainCommandDeliverer = services.GetService<DomainCommandDeliverer>()!;

            await domainCommandDeliverer.DeliverCommandAsync(firstStateCommand, headers);
            await domainCommandDeliverer.DeliverCommandAsync(secondStateCommand, headers);

            var deliveredEventContext = services.GetService<DeliveredEventContext>();
            deliveredEventContext!.Events.Count.Should().Be(2);
            EventPayload eventPayload = deliveredEventContext!.Events.Last();
            eventPayload.Id.Should().NotBe(Guid.Empty);
            eventPayload.StreamId.Should().Be(secondStateCommand.Id.ToString());
            eventPayload.CreatedOn.Should().BeWithin(TimeSpan.FromMinutes(10));
            eventPayload.Headers.Should().Equal(headers);
            eventPayload.EventIndex.Should().Be(1);
            eventPayload.EventBody.Should().BeOfType<TestStateTwoEvent>();
            eventPayload.EventBody.As<TestStateTwoEvent>().Id.Should().Be(secondStateCommand.Id.ToString());
        }

        [Fact]
        public async Task DeliveringThirdStateTestCommandResultsInEventWithStateViaAggregateWhenFirstAndSecondStateCommandAreDeliveredFirst()
        {
            var firstStateCommand = new TestStateOneCommand(Guid.NewGuid());
            var secondStateCommand = new TestStateTwoCommand(firstStateCommand.Id);
            var thirdStateCommand = new TestStateThreeCommand(firstStateCommand.Id);
            var headers = new Dictionary<string, object>();

            var domainCommandDeliverer = services.GetService<DomainCommandDeliverer>()!;

            await domainCommandDeliverer.DeliverCommandAsync(firstStateCommand, headers);
            await domainCommandDeliverer.DeliverCommandAsync(secondStateCommand, headers);
            await domainCommandDeliverer.DeliverCommandAsync(thirdStateCommand, headers);

            var deliveredEventContext = services.GetService<DeliveredEventContext>();
            deliveredEventContext!.Events.Count.Should().Be(4);
            EventPayload eventPayload = deliveredEventContext!.Events.Last();
            eventPayload.Id.Should().NotBe(Guid.Empty);
            eventPayload.StreamId.Should().Be(secondStateCommand.Id.ToString());
            eventPayload.CreatedOn.Should().BeWithin(TimeSpan.FromMinutes(10));
            eventPayload.Headers.Should().Equal(headers);
            eventPayload.EventIndex.Should().Be(3);
            eventPayload.EventBody.Should().BeOfType<TestStateThreeOtherEvent>();
            var testStateThreeEvent = eventPayload.EventBody.As<TestStateThreeOtherEvent>();
            testStateThreeEvent.Id.Should().Be(thirdStateCommand.Id.ToString());
            testStateThreeEvent.NumberOfEventsPrior.Should().Be(2);
        }
    }
}