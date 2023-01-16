namespace Interstellar
{
    using System;

    public class CommandNotHandledByAggregateException<TCommand, TState> : Exception
        where TState : AggregateState, new()
    {
        public CommandNotHandledByAggregateException(AggregateRoot<TState> aggregate) 
            : base($"{typeof(TCommand).Name} is not currently handled by {aggregate.GetType().Name} with id {aggregate.StreamId}. Current state is {aggregate.StatusState}")
        {
        }
    }
}