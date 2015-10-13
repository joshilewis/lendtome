using System;

namespace Lending.Domain
{
    public interface IAuthenticatedCommandHandler<in TCommand, out TResponse> : ICommandHandler<TCommand, TResponse>
        where TCommand : AuthenticatedCommand
    {
    }
}
