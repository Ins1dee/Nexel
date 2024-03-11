using MediatR;
using Nexel.Domain.Shared;

namespace Nexel.Application.Abstractions.Messaging;

public interface IQuery<TResponce> : IRequest<Result<TResponce>>
{
}