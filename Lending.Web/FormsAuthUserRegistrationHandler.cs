using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Lending.Domain;
using Lending.Execution.Auth;
using NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Web
{
    public class FormsAuthUserRegistrationHandler : UserRegistrationHandler
    {
        public FormsAuthUserRegistrationHandler(Func<ISession> sessionFunc, Func<IRepository> getRepository, Func<Guid> guidFunc)
            : base(sessionFunc, getRepository, guidFunc)
        { }

        public override BaseResponse HandleRequest(IAuthSession request)
        {
            FormsAuthentication.SetAuthCookie(request.DisplayName, true);

            return base.HandleRequest(request);
        }
    }
}