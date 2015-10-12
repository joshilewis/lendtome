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
    public class AuthenticatedWebserviceBase<TRequest, TResponse> : Service, IWebserviceBase<TRequest, TResponse> where TRequest : AuthenticatedRequest
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

            int authUserId = int.Parse(this.GetSession().Id);

            TResponse response = default(TResponse);
                unitOfWork.DoInTransaction(() =>
                {
                    ISession session = unitOfWork.CurrentSession;
                    ServiceStackUser user = session.QueryOver<ServiceStackUser>()
                    .Where(x => x.AuthenticatedUserId == authUserId)
                    .SingleOrDefault();

                    response = requestHandler.HandleRequest(request);
                });

            return response;
        }

    }
}
