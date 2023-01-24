namespace Interstellar
{
    using System;

    public class CannotLookupMessageTypeByNameException : InterstellarException
    {
        public CannotLookupMessageTypeByNameException(string messageName)
            : base($"Message type {messageName} does not exist in lookup")
        {
        }
    }
}