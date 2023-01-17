namespace Interstellar
{
    public class AggregateState
    {
        private readonly ApplyMethodConventionEventToHandlerRouter eventRouter;

        protected AggregateState()
        {
            eventRouter = new ApplyMethodConventionEventToHandlerRouter(this);
        }

        internal void ReplayEvent(EventPayload toReplay)
        {
            eventRouter.RouteEventToHandlers(toReplay.EventBody);
        }
    }
}