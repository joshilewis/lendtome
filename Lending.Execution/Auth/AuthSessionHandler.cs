using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain;
using Lending.Domain.Model;
using Lending.Domain.RegisterUser;
using NHibernate;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class AuthSessionHandler
    {
        private readonly Func<ISession> getSession;
        private readonly Func<Guid> getNewGuid;
        private readonly IMessageHandler<RegisterUser, Result> registerUserHandler;

        public AuthSessionHandler(Func<ISession> sessionFunc, Func<Guid> guidFunc,
            IMessageHandler<RegisterUser, Result> registerUserHandler)
        {
            this.getSession = sessionFunc;
            this.getNewGuid = guidFunc;
            this.registerUserHandler = registerUserHandler;
        }

        public virtual Result Handle(IAuthSession command)
        {
            int userAuthId = int.Parse(command.UserAuthId);
            ISession session = getSession();

            RegisteredUser registeredUser = session.QueryOver<RegisteredUser>()
                .Where(x => x.AuthUserId == userAuthId)
                .SingleOrDefault()
                ;

            Result result = new Result();

            if (registeredUser == null) //user doesn't exist yet, first time registration
            {
                UserAuthPersistenceDto userAuth = session.Get<UserAuthPersistenceDto>(userAuthId);

                Guid newUserId = getNewGuid();

                result = registerUserHandler.Handle(new RegisterUser(Guid.Empty, newUserId, userAuthId, userAuth.DisplayName,
                    userAuth.PrimaryEmail));

            }

            if (!result.Success) return result;

            return new Result();
        }
    }
}
