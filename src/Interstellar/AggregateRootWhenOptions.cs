namespace Interstellar
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AggregateRootWhenOptions<TCommand, TAggregateRootState> : IWhenOptions
        where TCommand : class
        where TAggregateRootState : AggregateState, new()
    {
        private readonly AggregateRoot<TAggregateRootState> root;
        private Action<object>? actioner;
        private Func<object, Task>? asyncActioner;

        public AggregateRootWhenOptions(AggregateRoot<TAggregateRootState> root)
        {
            this.root = root;
        }

        public void Then<TEvent>(Func<TCommand, TEvent> eventFactory)
        {
            Then(command => (eventFactory(command).Yield() as IEnumerable<object>)!);
        }

        public void Then(Func<TCommand, IEnumerable<object>> eventsFactory)
        {
            actioner = m =>
            {
                foreach (object @event in eventsFactory((m as TCommand)!))
                {
                    root.Then(@event);
                }
            };
        }

        public void Do(Action<TCommand> onDo)
        {
            actioner = m =>
            {
                onDo((m as TCommand)!);
            };
        }

        public void DoAsync(Func<TCommand, Task> onDo)
        {
            asyncActioner = m => onDo((m as TCommand)!);
        }

        public async Task HandleAsync(object toHandle)
        {
            actioner?.Invoke(toHandle);

            if (asyncActioner != null)
            {
                await asyncActioner(toHandle).ConfigureAwait(false);
            }
        }
    }
}