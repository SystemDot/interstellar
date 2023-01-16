
namespace Interstellar
{

    public abstract class AggregateRoot : AggregateRoot<NullState>
    {
    }
    public abstract class AggregateRoot<TState> where TState : AggregateRootState
    {
        protected TState State { get; }

        protected StatusChangeOptions On<TEvent>()
        {
            return new StatusChangeOptions();
        }

        protected WhenActions<TCommand> When<TCommand>()
        {
            return new WhenActions<TCommand>();
        }

        protected void Then<TEvent>(TEvent @event)
        {

        }
    }
}