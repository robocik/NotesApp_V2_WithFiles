using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace NoteBookApp.Server.Infrastructure
{
    public interface INotifierMediatorService
    {
        Task Notify(object obj);

        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }

    public class NotifierMediatorService : INotifierMediatorService
    {
        private readonly IMediator _mediator;

        public NotifierMediatorService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Notify(object notify)
        {
            return _mediator.Publish(notify);
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return _mediator.Send(request, cancellationToken);
        }
    }
}