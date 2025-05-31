using Invx.SharedKernel.Domain.Primitives.Results;
using MediatR;

namespace Invx.SharedKernel.Application.Abstractions.Messaging;
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
