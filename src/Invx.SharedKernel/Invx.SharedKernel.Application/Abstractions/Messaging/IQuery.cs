using Invx.SharedKernel.Domain.Primitives.Results;
using MediatR;

namespace Invx.SharedKernel.Application.Abstractions.Messaging;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
    Guid CorrelationId { get; }
}
