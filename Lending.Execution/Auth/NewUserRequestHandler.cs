﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Domain;
using NHibernate;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class NewUserRequestHandler : IRequestHandler<IAuthSession, BaseResponse>
    {
        private readonly Func<ISession> getSession;
        private readonly IRepository repository;

        public NewUserRequestHandler(Func<ISession> sessionFunc, IRepository repository)
        {
            this.getSession = sessionFunc;
            this.repository = repository;
        }

        protected NewUserRequestHandler() { }

        public virtual BaseResponse HandleRequest(IAuthSession request)
        {
            int userAuthId = int.Parse(request.UserAuthId);
            ISession session = getSession();

            var user = session.QueryOver<ServiceStackUser>()
                .Where(x => x.AuthenticatedUserId == userAuthId)
                .SingleOrDefault()
                ;

            if (user == null) //user doesn't exist yet, first time registration
            {
                UserAuthPersistenceDto auth = session.Get<UserAuthPersistenceDto>(userAuthId);
                //user = ServiceStackUser.Create(auth);
                session.Save(user);
                //repository.EmitEvent("User-" + user.Id, new UserAdded(user.Id, user.UserName, user.EmailAddress));
            }

            return new BaseResponse();
        }
    }
}
