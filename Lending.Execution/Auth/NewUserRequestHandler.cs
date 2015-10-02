using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Domain;
using Lending.Domain.Model;
using NHibernate;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class NewUserRequestHandler : IRequestHandler<IAuthSession, BaseResponse>
    {
        private readonly Func<ISession> getSession;
        private readonly IRepository repository;
        private readonly Func<Guid> getNewGuid; 

        public NewUserRequestHandler(Func<ISession> sessionFunc, IRepository repository, Func<Guid> guidFunc)
        {
            this.getSession = sessionFunc;
            this.repository = repository;
            this.getNewGuid = guidFunc;
        }

        protected NewUserRequestHandler() { }

        public virtual BaseResponse HandleRequest(IAuthSession request)
        {
            int userAuthId = int.Parse(request.UserAuthId);
            ISession session = getSession();

            ServiceStackUser serviceStackUser = session.QueryOver<ServiceStackUser>()
                .Where(x => x.AuthenticatedUserId == userAuthId)
                .SingleOrDefault()
                ;

            if (serviceStackUser == null) //user doesn't exist yet, first time registration
            {
                Guid newUserId = getNewGuid();
                serviceStackUser = new ServiceStackUser(userAuthId, newUserId);
                session.Save(serviceStackUser);

                UserAuthPersistenceDto userAuth = session.Get<UserAuthPersistenceDto>(userAuthId);

                User user = User.Create(newUserId, userAuth.DisplayName, userAuth.PrimaryEmail);
                repository.Save(user);
            }

            return new BaseResponse();
        }
    }
}
