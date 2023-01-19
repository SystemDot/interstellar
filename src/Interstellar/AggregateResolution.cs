using System.Collections.Generic;

namespace Interstellar
{
    public class AggregateResolution
    {
        public IAggregateRoot Aggregate { get; }
        public IEnumerable<string> StreamIds { get; }

        public AggregateResolution(IAggregateRoot aggregate, IEnumerable<string> streamIds)
        {
            Aggregate = aggregate;
            StreamIds = streamIds;
        }
    }
}