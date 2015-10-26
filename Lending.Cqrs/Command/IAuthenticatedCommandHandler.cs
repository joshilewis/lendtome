using Lending.Cqrs.Query;

namespace Lending.Cqrs.Command
{
    public interface IAuthenticatedCommandHandler<in TCommand, out TResult> : ICommandHandler<TCommand, TResult>,
        IAuthenticatedMessageHandler<TCommand, TResult> 
        where TCommand : AuthenticatedCommand where TResult : Result
    {
    }
}
