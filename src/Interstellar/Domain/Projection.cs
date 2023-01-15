using System.Threading.Tasks;

namespace Interstellar.Domain
{

    public abstract class Projection<TEvent>
    {
        protected abstract Task Project(TEvent @event);
    }
}