using System;

namespace Interstellar
{
    public class NoAggregateIsConfiguredToReceiveCommand<TCommand> : InterstellarException
    {
        public NoAggregateIsConfiguredToReceiveCommand() 
            : base($"No aggregate is configured to receive command {typeof(TCommand).Name}")
        {
            
        }
    }
}