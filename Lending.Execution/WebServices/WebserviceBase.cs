using System;
using System.Net;
using Lending.Core;
using Lending.Execution.UnitOfWork;
using ServiceStack.Logging;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Lending.Execution.WebServices
{
    public interface IWebserviceBase<TRequest, TResponse>
    { }

    [Authenticate]
    public class WebserviceBase<TRequest, TResponse> : Service, IWebserviceBase<TRequest, TResponse>
    {
        private readonly static ILog Log = LogManager.GetLogger(typeof(WebserviceBase<TRequest, TResponse>).FullName);

        private readonly IUnitOfWork unitOfWork;
        private readonly IRequestHandler<TRequest, TResponse> requestHandler;

        public WebserviceBase(IUnitOfWork unitOfWork, 
            IRequestHandler<TRequest, TResponse> requestHandler)
        {
            this.unitOfWork = unitOfWork;
            this.requestHandler = requestHandler;
        }

        public virtual object Any(TRequest request)
        {
            Log.InfoFormat("Received a request of type {0}", typeof(TRequest));
            TResponse response = default(TResponse);
                unitOfWork.DoInTransaction(() =>
                {
                    response = requestHandler.HandleRequest(request);
                });

            return response;
        }

    }
}
