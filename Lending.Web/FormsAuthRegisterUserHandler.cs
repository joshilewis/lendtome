using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Lending.Cqrs;
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

        public override Result HandleCommand(IAuthSession command)
        {
            FormsAuthentication.SetAuthCookie(command.DisplayName, true);

            return base.HandleCommand(command);
        }
    }
}