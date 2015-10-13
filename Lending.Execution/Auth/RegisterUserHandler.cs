using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Domain;
using Lending.Domain.Model;
using Lending.Domain.Persistence;
using NHibernate;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class RegisterUserHandler : ICommandHandler<IAuthSession, BaseResponse>
    {
        private readonly Func<ISession> getSession;
        private readonly Func<IRepository> getRepository;
        private readonly Func<Guid> getNewGuid; 

        public RegisterUserHandler(Func<ISession> sessionFunc, Func<IRepository> repositoryFunc, Func<Guid> guidFunc)
        {
            this.getSession = sessionFunc;
            this.getRepository = repositoryFunc;
            this.getNewGuid = guidFunc;
        }

        protected RegisterUserHandler() { }

        public virtual BaseResponse HandleCommand(IAuthSession request)
        {
            int userAuthId = int.Parse(request.UserAuthId);
            ISession session = getSession();

            ServiceStackUser serviceStackUser = session.QueryOver<ServiceStackUser>()
                .Where(x => x.AuthenticatedUserId == userAuthId)
                .SingleOrDefault()
                ;

            if (serviceStackUser == null) //user doesn't exist yet, first time registration
            {
                UserAuthPersistenceDto userAuth = session.Get<UserAuthPersistenceDto>(userAuthId);

                Guid newUserId = getNewGuid();
                serviceStackUser = new ServiceStackUser(userAuthId, newUserId, userAuth.DisplayName);
                session.Save(serviceStackUser);

                User user = User.Register(newUserId, userAuth.DisplayName, userAuth.PrimaryEmail);
                getRepository().Save(user);
            }

            return new BaseResponse();
        }
    }
}
