using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Domain;
using Lending.Domain.Model;
using NHibernate;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class RegisterUserHandler : ICommandHandler<IAuthSession, Result>
    {
        private readonly Func<ISession> getSession;
        private readonly Func<IEventRepository> getRepository;
        private readonly Func<Guid> getNewGuid; 

        public RegisterUserHandler(Func<ISession> sessionFunc, Func<IEventRepository> repositoryFunc, Func<Guid> guidFunc)
        {
            this.getSession = sessionFunc;
            this.getRepository = repositoryFunc;
            this.getNewGuid = guidFunc;
        }

        protected RegisterUserHandler() { }

        public virtual Result HandleCommand(IAuthSession command)
        {
            int userAuthId = int.Parse(command.UserAuthId);
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

                User user = User.Register(Guid.Empty, newUserId, userAuth.DisplayName, userAuth.PrimaryEmail);
                getRepository().Save(user);
            }

            return new Result();
        }
    }
}
