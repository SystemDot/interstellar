using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Interstellar;

namespace Interstellar.EventStorage.EventStore
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreConnectionProvider connectionProvider;

        public EventStore(IEventStoreConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        public Task<EventStream> GetEventsAsync(string streamId)
        {
            throw new NotImplementedException();
        }

        public async Task StoreEventsAsync(EventStreamSlice toStore)
        {
            IEventStoreConnection connection = await connectionProvider.ProvideConnectionAsync();

            await connection.AppendToStreamAsync(
                toStore.StreamId,
                toStore.StartIndex,
                toStore.Events.ToEventDataItems(),
                connectionProvider.ProvideUserCredentials());
        }

        public Task RedeliverAllEventsAsync()
        {
            throw new NotImplementedException();
        }
    }
}

public interface IEventStoreConnectionProvider
{
    Task<IEventStoreConnection> ProvideConnectionAsync();
    
    UserCredentials ProvideUserCredentials();
}

public static class EventPayloadExtensions
{
    public static IEnumerable<EventData> ToEventDataItems(this IEnumerable<EventPayload> from)
    {
        throw new NotImplementedException();
    }
}