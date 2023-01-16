using System;
using System.Collections.Generic;
using System.Linq;

namespace Interstellar
{
    public class AggregateCommandRegistration<TAggregate, TCommand> : IAggregateCommandRegistration
        where TAggregate : AggregateRoot where TCommand : class
    {
        private readonly List<Func<TCommand, string>> streamIdFactories;

        public AggregateCommandRegistration(Func<TCommand, string> streamIdFactory)
        {
            streamIdFactories = new List<Func<TCommand, string>>()
            {
                streamIdFactory
            };
        }

        public void LookUpAggregateWithJoinedStreams(object otherStreamIdFactories)
        {
            streamIdFactories.AddRange((otherStreamIdFactories as IEnumerable<Func<TCommand, string>>)!);
        }

        public AggregateResolution Resolve(object command, AggregateFactory aggregateFactory)
        {
            TAggregate aggregateRoot = aggregateFactory.CreateAggregate<TAggregate>();
            var streamIds = streamIdFactories.Select(f => f((command as TCommand)!));

            return new AggregateResolution(aggregateRoot, streamIds);
        }
    }
}