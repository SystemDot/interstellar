using System.Threading.Tasks;

namespace Interstellar
{
    public class CommandDeliverer
    {
        private readonly AggregateFactory aggregateFactory;
        private readonly AggregateLoader aggregateLoader;

        public CommandDeliverer(AggregateFactory aggregateFactory, AggregateLoader aggregateLoader)
        {
            this.aggregateFactory = aggregateFactory;
            this.aggregateLoader = aggregateLoader;
        }

        public async Task DeliverCommandAsync<TCommand>(TCommand command)
        {
            AggregateResolution aggregateResolution = AggregateLookupContext.Current.ResolveToAggregate(
                command,
                aggregateFactory);

            await aggregateResolution.HydrateAggregateAsync(aggregateLoader);
        }
    }
}