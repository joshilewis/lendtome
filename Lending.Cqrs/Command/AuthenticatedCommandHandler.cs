using System;
using Lending.Cqrs.Query;

namespace Lending.Cqrs.Command
{
    public abstract class AuthenticatedCommandHandler<TRequest, TResult, TAggregate> :
        CommandHandler<TRequest, TResult, TAggregate>, IAuthenticatedCommandHandler<TRequest, TResult, TAggregate>
        where TRequest : AuthenticatedCommand where TResult : Result where TAggregate : Aggregate
    {
        protected AuthenticatedCommandHandler(Func<IRepository> repositoryFunc,
            Func<IEventRepository> eventRepositoryFunc)
            : base(repositoryFunc, eventRepositoryFunc)
        {
        }
    }
}
