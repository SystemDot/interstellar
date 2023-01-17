namespace Interstellar
{
    using System;
    using System.Collections.Generic;

    public static class UseMessageTypes
    {
        public static UseMessageTypesThatImplement<TImplement> ThatImplement<TImplement>() => 
            new UseMessageTypesThatImplement<TImplement>();
    }

    public class UseMessageTypesThatImplement<TImplement>
    {
        public UseMessageTypesThatImplementAndFromAssemblyOf<TImplement, TAssemblyOf> FromAssemblyContaining<TAssemblyOf>() => 
            new UseMessageTypesThatImplementAndFromAssemblyOf<TImplement, TAssemblyOf>();
    }

    public class UseMessageTypesThatImplementAndFromAssemblyOf<TImplement, TAssemblyOf> : UseMessageTypesThatImplement<TImplement>
    {
        public MessageNameTypeLookup Build() => 
            MessageNameTypeLookup.FromTypes(typeof(TAssemblyOf).FromSameAssemblyWhereImplements<TImplement>());
    }

    public class MessageNameTypeLookup
    {
        private readonly Dictionary<string, Type> inner;

        public static MessageNameTypeLookup FromTypes(IEnumerable<Type> messageTypes)
        {
            var inner = new Dictionary<string, Type>();

            foreach (Type messageType in messageTypes)
            {
                if (inner.ContainsKey(messageType.Name))
                {
                    throw new MessageTypeAlreadyAddedToLookupException(messageType.Name);
                }

                inner.Add(messageType.Name, messageType);
            }

            return new MessageNameTypeLookup(inner);
        }

        private MessageNameTypeLookup(Dictionary<string, Type> inner)
        {
            this.inner = inner;
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