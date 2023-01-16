using System;
using System.Threading.Tasks;

namespace Interstellar
{

    public class WhenActions<TCommand>
    {
        public void Then<TEvent>(Func<TCommand, TEvent> eventCreator)
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