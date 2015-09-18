using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Lending.Core;
using Lending.Core.NewUser;
using Lending.Execution.Auth;
using NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Web
{
    public class FormsAuthNewUserRequestHandler : NewUserRequestHandler
    {
        public FormsAuthNewUserRequestHandler(Func<ISession> sessionFunc, IEventEmitter<UserAdded> eventEmitter)
            : base(sessionFunc, eventEmitter)
        { }

        public override BaseResponse HandleRequest(IAuthSession request)
        {
            FormsAuthentication.SetAuthCookie(request.DisplayName, true);

            return base.HandleRequest(request);
        }
    }
}