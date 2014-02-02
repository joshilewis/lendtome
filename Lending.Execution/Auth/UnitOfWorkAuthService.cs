using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Execution.UnitOfWork;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class UnitOfWorkAuthService : AuthService
    {
        private readonly IUnitOfWork unitOfWork;

        public UnitOfWorkAuthService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public new object Post(ServiceStack.ServiceInterface.Auth.Auth request)
        {
            object response = null;

            unitOfWork.DoInTransaction(() =>
            {
                response = base.Post(request);
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
