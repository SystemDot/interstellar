using System.Collections.Generic;

namespace Interstellar
{
    public class AggregateResolution
    {
        public AggregateRoot Aggregate { get; }
        public IEnumerable<string> StreamIds { get; }

        public AggregateResolution(AggregateRoot aggregate, IEnumerable<string> streamIds)
        {
            Aggregate = aggregate;
            StreamIds = streamIds;
        }
    }
}