using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Lending.Domain
{
    public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    {
        private readonly Func<ISession> getSession;
        private readonly Func<IRepository> getRepository;

        protected RequestHandler(Func<ISession> sessionFunc, Func<IRepository> repositoryFunc)
        {
            this.getSession = sessionFunc;
            this.getRepository = repositoryFunc;
        }

        public abstract TResponse HandleRequest(TRequest request);

        protected ISession Session => getSession();

        protected IRepository Repository => getRepository();
    }
}
