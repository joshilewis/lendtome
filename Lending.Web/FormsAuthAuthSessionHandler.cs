using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain;
using Lending.Domain.RegisterUser;
using Lending.Execution.Auth;
using NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Web
{
    public class FormsAuthAuthSessionHandler : AuthSessionHandler
    {
        public FormsAuthAuthSessionHandler(Func<ISession> sessionFunc, Func<Guid> guidFunc,
            IMessageHandler<RegisterUser, Result> registerUserHandler)
            : base(sessionFunc, guidFunc, registerUserHandler)
        {
        }

        public override Result Handle(IAuthSession command)
        {
            FormsAuthentication.SetAuthCookie(command.DisplayName, true);

            return base.Handle(command);
        }

    }
}