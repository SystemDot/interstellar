namespace Interstellar
{
    using System.Threading.Tasks;

    public interface IEventDeliverer
    {
        Task DeliverAsync(EventPayload eventPayload);
    }
}