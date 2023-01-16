using System.Threading.Tasks;

namespace Interstellar
{

    public abstract class Projection<TEvent>
    {
        protected abstract Task Project(TEvent @event);
    }
}