using Invx.SharedKernel.Domain.Primitives.Results;
using MediatR;

namespace Invx.SharedKernel.Application.Abstractions.Messaging;
public interface ICommand : IRequest<Result>, IBaseCommand
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
{
}

public interface IBaseCommand
{
    Guid CorrelationId { get; }
}