namespace Interstellar
{
    using System.Threading;

    public static class AggregateLookupContext
    {
        private static readonly AsyncLocal<AggregateLookup> Storage = new AsyncLocal<AggregateLookup>();

        public static AggregateLookup Current => Storage.Value ?? (Storage.Value = new AggregateLookup());
    }
}