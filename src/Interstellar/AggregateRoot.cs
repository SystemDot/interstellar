
namespace Interstellar
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class AggregateRoot : AggregateRoot<NullState>
    {
    }

    public abstract class AggregateRoot<TState>
        where TState : AggregateState, new()
    {
        private readonly Dictionary<Type, IStatusChangeOptions> statusOptionsLookup;
        private readonly Dictionary<Type, IWhenOptions> whenOptionsLookup;
        public string StreamId { get; set; } = null!;
        protected TState State { get; }
        internal string? StatusState { get; private set; }

        protected AggregateRoot()
        {
            State = new TState();
            statusOptionsLookup = new Dictionary<Type, IStatusChangeOptions>();
            whenOptionsLookup = new Dictionary<Type, IWhenOptions>();
        }

        internal void ReplayEvents(IEnumerable<EventPayload> toReplay)
        {
            long lastEventIndex = UnitOfWorkContext.Current!.EventsAdded.StartIndex;

            foreach (EventPayload eventPayload in toReplay)
            {
                if (eventPayload.StreamId == StreamId)
                {
                    lastEventIndex = eventPayload.EventIndex;
                }

                ReplayEvent(eventPayload);
            }

            UnitOfWorkContext.Current.EventsAdded = new EventStreamSlice(StreamId!, lastEventIndex);
        }

        private void ReplayEvent(EventPayload toReplay)
        {
            State.ReplayEvent(toReplay);
            RunBecome(toReplay.Body);
        }

        private void RunBecome(object @event)
        {
            if (!statusOptionsLookup.ContainsKey(@event.GetType()))
            {
                return;
            }

            whenOptionsLookup.Clear();
            StatusState = statusOptionsLookup[@event.GetType()].RunBecome();
        }

        internal Task ReceiveCommandAsync<TCommand>(TCommand command)
        {
            if (!whenOptionsLookup.ContainsKey(command!.GetType()))
            {
                throw new CommandNotHandledByAggregateException<TCommand, TState>(this);
            }

            return whenOptionsLookup[command.GetType()].HandleAsync(command);
        }

        protected StatusChangeOptions On<TEvent>()
        {
            var options = new StatusChangeOptions();
            statusOptionsLookup[typeof(TEvent)] = options;
            return options;
        }

        protected AggregateRootWhenOptions<TCommand, TState> When<TCommand>()
            where TCommand : class
        {
            var whenOptions = new AggregateRootWhenOptions<TCommand, TState>(this);
            whenOptionsLookup[typeof(TCommand)] = whenOptions;
            return whenOptions;
        }

        protected internal void Then<TEvent>(TEvent @event)
        {
            UnitOfWorkContext.Current.EventsAdded = UnitOfWorkContext.Current.EventsAdded.AddEvent(@event!);
        }
    }
}

