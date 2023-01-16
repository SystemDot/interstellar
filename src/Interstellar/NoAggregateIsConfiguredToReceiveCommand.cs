using System;

namespace Interstellar
{
    public class NoAggregateIsConfiguredToReceiveCommand<TCommand> : Exception
    {
        public NoAggregateIsConfiguredToReceiveCommand() 
            : base($"No aggregate is configured to receive command {typeof(TCommand).Name}")
        {
            
        }
    }
}