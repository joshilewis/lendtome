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

        protected CommandHandler(Func<ISession> sessionFunc, Func<IRepository> repositoryFunc)
        {
            this.getSession = sessionFunc;
            this.getRepository = repositoryFunc;
        }

        public abstract TResponse HandleCommand(TRequest request);

        protected ISession Session => getSession();

        protected IRepository Repository => getRepository();
    }
}
