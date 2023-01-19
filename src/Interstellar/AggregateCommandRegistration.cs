using System;
using System.Collections.Generic;
using System.Linq;

namespace Interstellar
{
    public class AggregateCommandRegistration<TAggregate, TAggregateState, TCommand> : IAggregateCommandRegistration
        where TAggregate : AggregateRoot<TAggregateState>
        where TAggregateState : AggregateState, new()
        where TCommand : class
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
            TAggregate aggregateRoot = aggregateFactory.CreateAggregate<TAggregate, TAggregateState>();

            IEnumerable<string> streamIds = streamIdFactories
                .Select(f => f((command as TCommand)!))
                .ToArray();

            aggregateRoot.StreamId = streamIds.First();

            return new AggregateResolution(aggregateRoot, streamIds);
        }
    }
}