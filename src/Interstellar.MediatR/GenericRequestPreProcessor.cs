using MediatR.Pipeline;

namespace Interstellar.MediatR
{
    public class GenericRequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
    {
        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"pre proc {request.GetType().Name}");
            return Task.CompletedTask;
        }
    }
}