using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core;
using Lending.Core.NewUser;
using NHibernate;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class NewUserRequestHandler : IRequestHandler<IAuthSession, BaseResponse>
    {
        private readonly Func<ISession> getSession;
        private readonly IEventEmitter eventEmitter;

        public NewUserRequestHandler(Func<ISession> sessionFunc, IEventEmitter eventEmitter)
        {
            this.getSession = sessionFunc;
            this.eventEmitter = eventEmitter;
        }

        protected NewUserRequestHandler() { }

        public virtual BaseResponse HandleRequest(IAuthSession request)
        {
            int userAuthId = int.Parse(request.UserAuthId);
            ISession session = getSession();

            var user = session.QueryOver<ServiceStackUser>()
                .JoinQueryOver(x => x.AuthenticatedUser)
                .Where(x => x.Id == userAuthId)
                .SingleOrDefault()
                ;

            if (user == null) //user doesn't exist yet, first time registration
            {
                UserAuthPersistenceDto auth = session.Get<UserAuthPersistenceDto>(userAuthId);
                user = new ServiceStackUser(auth);
                session.Save(user);
                eventEmitter.EmitEvent(new UserAdded(user.Id, user.UserName, user.EmailAddress));
            }

            return new BaseResponse();
        }
    }
}
