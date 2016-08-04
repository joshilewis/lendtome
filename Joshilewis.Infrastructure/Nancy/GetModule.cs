using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Query;
using Joshilewis.Infrastructure.Auth;
using Joshilewis.Infrastructure.UnitOfWork;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Joshilewis.Infrastructure.Nancy
{
    public abstract class GetModule<TQuery> : NancyModule where TQuery : Query
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMessageHandler<TQuery> messageHandler;

        protected GetModule(IUnitOfWork unitOfWork, IMessageHandler<TQuery> messageHandler, string path)
        {
            this.unitOfWork = unitOfWork;
            this.messageHandler = messageHandler;

            this.RequiresHttps();

            Get[path] = _ =>
            {
                TQuery request = this.Bind<TQuery>();

                if (Context.CurrentUser != null)
                {
                    CustomUserIdentity user = this.Context.CurrentUser as CustomUserIdentity;
                    request.UserId = user.Id;
                }

                object response = null;
                unitOfWork.DoInTransaction(() =>
                {
                    response = messageHandler.Handle(request);

                });

                return response;
            };
        }


    }
}
