using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core;
using NHibernate;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class NewUserRequestHandler : IRequestHandler<IAuthSession, BaseResponse>
    {
        private readonly Func<ISession> getSession;

        public NewUserRequestHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
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
            }

            return new BaseResponse();
        }
    }
}
