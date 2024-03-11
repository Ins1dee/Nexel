using MediatR;
using Nexel.Domain.Shared;

namespace Nexel.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponce> : IRequestHandler<TQuery, Result<TResponce>>
    where TQuery : IQuery<TResponce>
{
}