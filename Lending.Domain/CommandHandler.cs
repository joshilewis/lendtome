using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Lending.Domain
{
    public abstract class CommandHandler<TRequest, TResponse> : ICommandHandler<TRequest, TResponse>
    {
        private readonly Func<ISession> getSession;
        private readonly Func<IRepository> getRepository;

        protected CommandHandler(Func<ISession> getSession, Func<IRepository> getRepository,
            ICommandHandler<TRequest, TResponse> nextHandler)
        {
            this.getSession = getSession;
            this.getRepository = getRepository;
            this.NextHandler = nextHandler;
        }

        public abstract TResponse HandleCommand(TRequest command);

        protected ISession Session => getSession();

        protected IRepository Repository => getRepository();

        protected ICommandHandler<TRequest, TResponse> NextHandler { get; }
    }
}
