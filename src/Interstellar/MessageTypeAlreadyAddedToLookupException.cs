namespace Interstellar
{
    using System;

    public class MessageTypeAlreadyAddedToLookupException : Exception
    {
        public MessageTypeAlreadyAddedToLookupException(string messageName)
            :base($"Message type {messageName} already added to lookup")
        {
        }
    }
}