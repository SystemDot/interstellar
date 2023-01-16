namespace Interstellar
{
    using System;

    public class UnitOfWork : IDisposable
    {
        public EventStreamSlice EventsAdded { get; set; }

        public UnitOfWork(string streamId)
        {
            EventsAdded = new EventStreamSlice(streamId!);
            UnitOfWorkContext.Current = this;
        }

        public void Dispose()
        {
            UnitOfWorkContext.Current = null;
        }
    }
}