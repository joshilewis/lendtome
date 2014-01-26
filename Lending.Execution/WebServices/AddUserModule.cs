using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core;
using Lending.Core.AddUser;
using Lending.Execution.UnitOfWork;
using Nancy;
using Nancy.ModelBinding;

namespace Lending.Execution.WebServices
{
    public class AddUserModule : NancyModule
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRequestHandler<AddUserRequest, BaseResponse> requestHandler;

        public AddUserModule(IUnitOfWork unitOfWork, 
            IRequestHandler<AddUserRequest, BaseResponse> requestHandler)
        {
            this.requestHandler = requestHandler;
            this.unitOfWork = unitOfWork;

            Get["/user/add"] = _ =>
            {
                var request = this.Bind<AddUserRequest>();
                BaseResponse response = null;

                unitOfWork.DoInTransaction(() =>
                {
                    response = requestHandler.HandleRequest(request);
                });

                return response;
            };
            //Execute(request)
        }

        private BaseResponse Execute(AddUserRequest request)
        {
            BaseResponse response = null;

            try
            {
                unitOfWork.DoInTransaction(() =>
                {
                    response = requestHandler.HandleRequest(request);
                });
            }
            catch (Exception ex)
            {
                //Log.Error("Exception thrown while executing webservice request", ex);
                //response = ResponseStatusTranslator.CreateErrorResponse(HttpStatusCode.InternalServerError.ToString(),
                //    "There was an internal server error. Please try again later.");
            }

            return response;
        }
    }
}
