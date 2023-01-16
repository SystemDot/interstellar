namespace Interstellar
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class ConventionEventToHandlerRouter
    {
        private readonly List<SourcedEventHandler> eventHandlers;
        private readonly string methodName;
        private readonly object target;

        protected ConventionEventToHandlerRouter(object target, string methodName)
        {
            this.target = target;
            this.methodName = methodName;
            eventHandlers = new List<SourcedEventHandler>(GetEventHandlers());
        }

        public void RouteEventToHandlers(object @event)
        {
            foreach (SourcedEventHandler handler in eventHandlers)
            {
                if (handler.Handle(@event))
                {
                    break;
                }
            }
        }

        private IEnumerable<SourcedEventHandler> GetEventHandlers() =>
            GetEventHandlersFromTarget().Select(CreateSourcedEventHandler).ToList();

        private IEnumerable<MethodInfo> GetEventHandlersFromTarget()
        {
            return target.GetType()
                .GetMethods()
                .Where(m => m.Name == methodName && m.GetParameters().Length == 1);
        }

        private SourcedEventHandler CreateSourcedEventHandler(MethodInfo method) =>
            new SourcedEventHandler(GetFirstParameterType(method), GetHandler(method));

        private Action<object> GetHandler(MethodInfo method)
        {
            return e => method.Invoke(
                target,
                new[]
                {
                    e
                });
        }

        private static Type GetFirstParameterType(MethodInfo method) =>
            method.GetParameters().First().ParameterType;
    }
}