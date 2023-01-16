namespace Interstellar
{
    using System.Threading.Tasks;

    public interface IEventStore
    {
        Task<EventStream> GetEventsAsync(string streamId);

        Task StoreEventsAsync(EventStreamSlice toStore);
    }
}