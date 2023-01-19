using System;
using System.Collections.Generic;

namespace Interstellar
{
    public class AggregateLookup
    {
        private readonly Dictionary<Type, IAggregateCommandRegistration> inner;

        public AggregateLookup()
        {
            inner = new Dictionary<Type, IAggregateCommandRegistration>();
        }

        public void RegisterCommandForAggregateReception<TAggregate, TCommand>(Func<TCommand, string> streamIdFactory)
            where TAggregate : AggregateRoot
            where TCommand : class
        {
            inner.Add(typeof(TCommand), new AggregateCommandRegistration<TAggregate, TCommand>(streamIdFactory));
        }

        public void SetAggregateToLookUpWithJoinedStreams<TCommand>(Func<TCommand, string>[] otherStreamIdFactories)
        {
            inner[typeof(TCommand)].LookUpAggregateWithJoinedStreams(otherStreamIdFactories);
        }

        public AggregateResolution ResolveToAggregate<TCommand>(TCommand command, AggregateFactory aggregateFactory)
        {
            if (!inner.ContainsKey(typeof(TCommand)))
            {
                throw new NoAggregateIsConfiguredToReceiveCommand<TCommand>();
            }

            IAggregateCommandRegistration registration = inner[typeof(TCommand)];

            return registration.Resolve(command!, aggregateFactory);
        }

        public bool CommandIsReceivedByAnAggregate<TCommand>() => inner.ContainsKey(typeof(TCommand));
    }
}