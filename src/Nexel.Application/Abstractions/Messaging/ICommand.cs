using MediatR;
using Nexel.Domain.Shared;

namespace Nexel.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>, ICommandBase
{
}

public interface ICommand<TResponce> : IRequest<Result<TResponce>>, ICommandBase
{
}

public interface ICommandBase
{
}