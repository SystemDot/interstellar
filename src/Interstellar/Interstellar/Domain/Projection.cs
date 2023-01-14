using System.Threading.Tasks;
using Interstellar.Messaging;

namespace Interstellar.Domain
{

    public abstract class Projection<TEvent> where TEvent : IEvent
    {
        protected abstract Task Project(TEvent @event);
    }
}