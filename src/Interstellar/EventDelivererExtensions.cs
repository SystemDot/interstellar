using System.Threading.Tasks;

namespace Interstellar
{
    public static class EventDelivererExtensions
    {
        public static async Task DeliverEventsAsync(this IEventDeliverer eventDeliverer, EventStreamSlice toDeliver)
        {
            foreach (EventPayload? eventPayload in toDeliver)
            {
                await eventDeliverer.DeliverAsync(eventPayload);
            }
        }
    }
}