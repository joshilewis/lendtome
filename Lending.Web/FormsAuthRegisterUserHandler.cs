using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain;
using Lending.Execution.Auth;
using NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Web
{
    public class FormsAuthRegisterUserHandler : RegisterUserHandler
    {
        public FormsAuthRegisterUserHandler(Func<ISession> sessionFunc, Func<IEventRepository> getRepository, Func<Guid> guidFunc)
            : base(sessionFunc, getRepository, guidFunc)
        { }

        public override Result Handle(IAuthSession command)
        {
            FormsAuthentication.SetAuthCookie(command.DisplayName, true);

            return base.Handle(command);
        }
    }
}