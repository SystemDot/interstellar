namespace Interstellar
{
    using System;
    using System.Reflection;

    public class SourcedEventHandler
    {
        readonly Type eventType;
        readonly Action<object> handler;

        public SourcedEventHandler(Type eventType, Action<object> handler)
        {
            this.eventType = eventType;
            this.handler = handler;
        }

        public bool Handle(object? toHandle)
        {
            if (toHandle == null)
            {
                return false;
            }

            if (!eventType.GetTypeInfo().IsAssignableFrom(toHandle.GetType().GetTypeInfo()))
            {
                return false;
            }

            handler.Invoke(toHandle);
            return true;
        }

        public override string ToString() =>
            $"Handler for: {eventType}";
    }
}