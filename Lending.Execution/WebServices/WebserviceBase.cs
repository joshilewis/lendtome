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

    public class WebserviceBase<TRequest, TResponse> : ServiceBase<TRequest>, IWebserviceBase<TRequest, TResponse>
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

        protected override object Run(TRequest request)
        {
            Log.InfoFormat("Received a request of type {0}", typeof(TRequest));
            object response = null;
            try
            {
                unitOfWork.DoInTransaction(() =>
                {
                    response = requestHandler.HandleRequest(request);
                });
            }
            catch (Exception ex)
            {
                Log.Error("Exception thrown while executing webservice request", ex);
                response = ResponseStatusTranslator.CreateErrorResponse(HttpStatusCode.InternalServerError.ToString(),
                    "There was an internal server error. Please try again later.");
            }

            return response;
        }

    }
}
