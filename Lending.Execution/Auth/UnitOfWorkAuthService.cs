using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain;
using Lending.Execution.UnitOfWork;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class UnitOfWorkAuthService : AuthService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly RegisterUserHandler commandHandler;

        public UnitOfWorkAuthService(IUnitOfWork unitOfWork,
            RegisterUserHandler commandHandler)
        {
            this.unitOfWork = unitOfWork;
            this.commandHandler = commandHandler;
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
                    commandHandler.Handle(this.GetSession());
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
