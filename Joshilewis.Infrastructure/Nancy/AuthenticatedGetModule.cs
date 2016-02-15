using Joshilewis.Cqrs.Query;
using Joshilewis.Infrastructure.Auth;
using Joshilewis.Infrastructure.UnitOfWork;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Joshilewis.Infrastructure.Nancy
{
    public abstract class AuthenticatedGetModule<TQuery> : NancyModule where TQuery : AuthenticatedQuery
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticatedQueryHandler<TQuery> queryHandler;

        protected AuthenticatedGetModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<TQuery> queryHandler, string path)
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

                object response = null;
                unitOfWork.DoInTransaction(() =>
                {
                    response = queryHandler.Handle(query);

                });

                return response;
            };
        }


    }
}
