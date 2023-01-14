using System;
using Interstellar.Messaging;

namespace Interstellar.Domain.Bootstrapping
{
    public class DomainBootstrapping
    {
        public static DomainCommandBootstrapping<TCommand> Route<TCommand>() where TCommand : ICommand
        {
            throw new NotImplementedException();
        }
    }
}