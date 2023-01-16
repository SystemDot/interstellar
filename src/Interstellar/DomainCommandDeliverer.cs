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

        public async Task DeliverCommandAsync<TCommand>(TCommand command)
        {
            AggregateResolution aggregateResolution = AggregateLookupContext
                .Current
                .ResolveToAggregate(command, aggregateFactory);

            using var uow = new UnitOfWork(aggregateResolution.Aggregate.StreamId);
            IEnumerable<EventPayload> events = await eventStreamLoader.LoadEventsAsync(aggregateResolution.StreamIds);
            aggregateResolution.Aggregate.ReplayEvents(events);
            await aggregateResolution.Aggregate.ReceiveCommandAsync(command);
            await eventStore.StoreEventsAsync(uow.EventsAdded);
        }
    }
}