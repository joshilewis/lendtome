using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs.Query;
using Lending.Execution.Auth;
using Lending.Execution.UnitOfWork;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Lending.Execution.Modules
{
    public abstract class AuthenticatedGetModule<TQuery, TResult> : NancyModule where TQuery : AuthenticatedQuery where TResult : Result
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticatedQueryHandler<TQuery, TResult> queryHandler;

        protected AuthenticatedGetModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<TQuery, TResult> queryHandler, string path)
        {
            this.unitOfWork = unitOfWork;
            this.queryHandler = queryHandler;

            this.RequiresHttps();
            this.RequiresAuthentication();

            Get[path] = _ =>
            {
                CustomUserIdentity user = this.Context.CurrentUser as CustomUserIdentity;
                TQuery query = this.Bind<TQuery>();
                query.UserId = user.Id;

                Result response = default(Result);
                unitOfWork.DoInTransaction(() =>
                {
                    response = queryHandler.Handle(query);

                });

                return response;
            };
        }


    }
}
