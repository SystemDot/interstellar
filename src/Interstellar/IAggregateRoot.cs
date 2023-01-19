namespace Interstellar
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAggregateRoot
    {
        string StreamId { get; }

        void ReplayEvents(IEnumerable<EventPayload> events);

        Task ReceiveCommandAsync<TCommand>(TCommand command);
    }
}