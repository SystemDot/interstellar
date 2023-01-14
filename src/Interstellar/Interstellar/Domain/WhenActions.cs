using System;
using System.Threading.Tasks;
using Interstellar.Messaging;

namespace Interstellar.Domain
{

    public class WhenActions<TCommand>
        where TCommand : ICommand
    {
        public void Then<TEvent>(Func<TCommand, TEvent> eventCreator)
            where TEvent : IEvent
        {

        }

        public void Do(Action<TCommand> toDo)
        {

        }

        public void DoAsync(Func<TCommand, Task> toDo)
        {

        }
    }
}