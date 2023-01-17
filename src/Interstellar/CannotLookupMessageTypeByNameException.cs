namespace Interstellar
{
    using System;

    public class CannotLookupMessageTypeByNameException : Exception
    {
        public CannotLookupMessageTypeByNameException(string messageName)
            : base($"Message type {messageName} does not exist in lookup")
        {
        }
    }
}