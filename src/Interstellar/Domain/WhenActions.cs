using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Interstellar.Domain
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