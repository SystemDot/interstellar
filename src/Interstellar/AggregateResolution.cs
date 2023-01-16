using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interstellar
{
    public class AggregateResolution
    {
        private readonly AggregateRoot aggregate;
        private readonly IEnumerable<string> streamIds;

        public AggregateResolution(AggregateRoot aggregate, IEnumerable<string> streamIds)
        {
            this.aggregate = aggregate;
            this.streamIds = streamIds;
        }

        public Task HydrateAggregateAsync(AggregateLoader aggregateLoader)
        {
            return Task.CompletedTask;
        }
    }
}