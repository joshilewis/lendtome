using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Lending.Core;
using Lending.Execution.Auth;
using NHibernate;

namespace Lending.Web
{
    public class FormsAuthNewUserRequestHandler : NewUserRequestHandler
    {
        public FormsAuthNewUserRequestHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        { }

        public override BaseResponse HandleRequest(string request)
        {
            FormsAuthentication.SetAuthCookie(request, true);

            return base.HandleRequest(request);
        }
    }
}