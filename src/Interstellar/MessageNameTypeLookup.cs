namespace Interstellar
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MessageNameTypeLookup
    {
        public static MessageNameTypeLookup FromTypesFromAssemblyContainingAndImplements<TAssemblyOf, TImplements>() =>
            new MessageNameTypeLookup(typeof(TAssemblyOf)
                .FromSameAssemblyWhereImplements<TImplements>());

        public MessageNameTypeLookup AndFromTypesFromAssemblyContainingAndImplements<TAssemblyOf, TImplements>() =>
            new MessageNameTypeLookup(inner.Values.Concat(typeof(TAssemblyOf)
                .FromSameAssemblyWhereImplements<TImplements>()));

        private readonly Dictionary<string, Type> inner;

        private MessageNameTypeLookup(IEnumerable<Type> messageTypes)
        {
            inner = new Dictionary<string, Type>();
            
            foreach (Type messageType in messageTypes)
            {
                if (inner.ContainsKey(messageType.Name))
                {
                    throw new MessageTypeAlreadyAddedToLookupException(messageType.Name);
                }

                inner.Add(messageType.Name, messageType);
            }
        }

        public Type Lookup(string messageName)
        {
            if (!inner.ContainsKey(messageName))
            {
                throw new CannotLookupMessageTypeByNameException(messageName);
            }

            return inner[messageName];
        }
    }
}