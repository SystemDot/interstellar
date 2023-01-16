namespace Interstellar
{
    public interface IAggregateCommandRegistration
    {
        void LookUpAggregateWithJoinedStreams(object otherStreamIdFactories);
        AggregateResolution Resolve(object command, AggregateFactory aggregateFactory);
    }
}