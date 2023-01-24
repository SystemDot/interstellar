namespace Interstellar
{
    using System;

    public class MessageTypeAlreadyAddedToLookupException : InterstellarException
    {
        public MessageTypeAlreadyAddedToLookupException(string messageName)
            :base($"Message type {messageName} already added to lookup")
        {
        }
    }
}