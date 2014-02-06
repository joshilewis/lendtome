using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core;
using Lending.Execution.UnitOfWork;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class UnitOfWorkAuthService : AuthService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRequestHandler<string, BaseResponse> requestHandler;

        public UnitOfWorkAuthService(IUnitOfWork unitOfWork,
            IRequestHandler<string, BaseResponse> requestHandler)
        {
            this.unitOfWork = unitOfWork;
            this.requestHandler = requestHandler;
        }

        public new object Post(ServiceStack.ServiceInterface.Auth.Auth request)
        {
            object response = null;

            bool authenticatedAtStart = this.GetSession().IsAuthenticated;
            unitOfWork.DoInTransaction(() =>
            {
                response = base.Post(request);
                if (this.GetSession().IsAuthenticated && !authenticatedAtStart)
                    //User just authenticated, check if user exists
                {
                    requestHandler.HandleRequest(this.GetSession().UserAuthId);
                }
            });

            return response;
        }

        public new AuthResponse Authenticate(ServiceStack.ServiceInterface.Auth.Auth request)
        {
            return base.Authenticate(request);
        }

        public new object Get(ServiceStack.ServiceInterface.Auth.Auth request)
        {
            return Post(request);
        }
    }
}
