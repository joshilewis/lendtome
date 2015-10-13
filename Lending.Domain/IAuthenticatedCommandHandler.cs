using System;

namespace Lending.Domain
{
    public interface IAuthenticatedCommandHandler<in TCommand, out TResponse> where TCommand : AuthenticatedCommand
    {
        TResponse HandleCommand(TCommand request);
    }
}
