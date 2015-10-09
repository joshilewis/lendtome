using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Lending.Domain
{
    public abstract class BaseAuthenticatedRequestHandler<TRequest, TResponse> : IAuthenticatedRequestHandler<TRequest, TResponse>
    {
        private readonly Func<ISession> getSession;
        private readonly Func<IRepository> getRepository; 

        protected BaseAuthenticatedRequestHandler(Func<ISession> sessionFunc, Func<IRepository> repositoryFunc)
        {
            this.getSession = sessionFunc;
            this.getRepository = repositoryFunc;
        }

        protected ISession Session => getSession();

        protected IRepository Repository => getRepository();

        public abstract TResponse HandleRequest(TRequest request, Guid userId);
    }
}
