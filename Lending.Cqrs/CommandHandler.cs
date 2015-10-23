using System;
using NHibernate;

namespace Lending.Cqrs
{
    public abstract class CommandHandler<TRequest, TResponse> : ICommandHandler<TRequest, TResponse>
    {
        private readonly Func<ISession> getSession;
        private readonly Func<IRepository> getRepository;

        protected CommandHandler(Func<ISession> getSession, Func<IRepository> getRepository)
        {
            this.getSession = getSession;
            this.getRepository = getRepository;
        }

        public abstract TResponse HandleCommand(TRequest command);

        protected ISession Session => getSession();

        protected IRepository Repository => getRepository();

        protected virtual Result Success()
        {
            return new Result();
        }

        protected virtual Result Fail(string reason)
        {
            return new Result(reason);
        }

    }
}
