using System;

namespace Interstellar
{
    public abstract class InterstellarException : Exception
    {
        protected InterstellarException(string message) : base(message) { }
    }
}