using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interstellar
{
    public class DomainCommandDeliverer
    {
        private readonly AggregateFactory aggregateFactory;
        private readonly EventStreamLoader eventStreamLoader;
        private readonly IEventStore eventStore;

        public DomainCommandDeliverer(
            AggregateFactory aggregateFactory,
            EventStreamLoader eventStreamLoader,
            IEventStore eventStore)
        {
            this.aggregateFactory = aggregateFactory;
            this.eventStreamLoader = eventStreamLoader;
            this.eventStore = eventStore;
        }

        public Task DeliverCommandAsync<TCommand>(TCommand command) =>
            DeliverCommandAsync(command, 0);

        private async Task DeliverCommandAsync<TCommand>(TCommand command, int attempts)
        {
            AggregateResolution aggregateResolution = AggregateLookupContext
                .Current
                .ResolveToAggregate(command, aggregateFactory);

            using var uow = new UnitOfWork(aggregateResolution.Aggregate.StreamId);
            IEnumerable<EventPayload> events = await eventStreamLoader.LoadEventsAsync(aggregateResolution.StreamIds);
            aggregateResolution.Aggregate.ReplayEvents(events);
            await aggregateResolution.Aggregate.ReceiveCommandAsync(command);
            
            try
            {
                await eventStore.StoreEventsAsync(uow.EventsAdded);
            }
            catch (ExpectedEventIndexIncorrectException)
            {
                if (attempts == 3)
                {
                    throw;
                }

                await DeliverCommandAsync(command, ++attempts);
            }
        }
    }
}