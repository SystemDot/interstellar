using MediatR.Pipeline;

namespace Interstellar.MediatR
{
    public class GenericRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly CommandDeliverer deliverer;

        public GenericRequestPreProcessor(CommandDeliverer deliverer)
        {
            this.deliverer = deliverer;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            return deliverer.DeliverCommandAsync(request);
        }
    }
}