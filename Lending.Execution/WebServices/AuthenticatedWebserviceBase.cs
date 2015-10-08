using System;
using System.Net;
using Lending.Domain;
using Lending.Execution.Auth;
using Lending.Execution.UnitOfWork;
using NHibernate;
using ServiceStack.Logging;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Lending.Execution.WebServices
{
    public class AuthenticatedWebserviceBase<TRequest, TResponse> : Service, IWebserviceBase<TRequest, TResponse>
    {
        private readonly static ILog Log = LogManager.GetLogger(typeof(WebserviceBase<TRequest, TResponse>).FullName);

        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticatedRequestHandler<TRequest, TResponse> requestHandler;

        public AuthenticatedWebserviceBase(IUnitOfWork unitOfWork,
            IAuthenticatedRequestHandler<TRequest, TResponse> requestHandler)
        {
            this.unitOfWork = unitOfWork;
            this.requestHandler = requestHandler;
        }

        [Authenticate]
        public virtual object Any(TRequest request)
        {
            Log.InfoFormat("Received a request of type {0}", typeof(TRequest));

            ISession session = unitOfWork.CurrentSession;

            int authUserId = int.Parse(this.GetSession().Id);
            ServiceStackUser user = session.Get<ServiceStackUser>(authUserId);

            TResponse response = default(TResponse);
                unitOfWork.DoInTransaction(() =>
                {
                    response = requestHandler.HandleRequest(request, user.UserId);
                });

            return response;
        }

    }
}
