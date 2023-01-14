using Interstellar.Messaging;

namespace Interstellar.Domain
{

    public abstract class AggregateRoot : AggregateRoot<NullState>
    {
    }
    public abstract class AggregateRoot<TState> where TState : AggregateRootState
    {
        protected TState State { get; }

        protected StatusChangeOptions On<TEvent>() where TEvent : IEvent
        {
            return new StatusChangeOptions();
        }

        protected WhenActions<TCommand> When<TCommand>()
            where TCommand : ICommand
        {
            return new WhenActions<TCommand>();
        }

        protected void Then<TEvent>(TEvent @event)
            where TEvent : IEvent
        {

        }
    }
}