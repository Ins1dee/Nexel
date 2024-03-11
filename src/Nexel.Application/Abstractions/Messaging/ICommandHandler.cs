using MediatR;
using Nexel.Domain.Shared;

namespace Nexel.Application.Abstractions.Messaging;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResponce> : IRequestHandler<TCommand, Result<TResponce>>
    where TCommand : ICommand<TResponce>
{
}