using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Mvc;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using StructureMap;

namespace Lending.Web.Controllers
{
    public class HomeController : ServiceStackController
    {
        public ActionResult Index(dynamic request)
        {
            bool authed = Request.IsAuthenticated;
            var userSession = SessionFeature.GetOrCreateSession<AuthUserSession>(ObjectFactory.GetInstance<ICacheClient>());
            if (userSession != null)
            {
                FormsAuthentication.SetAuthCookie(userSession.Id, true);
            }
            return View();
        }

        [Authenticate]
        public ActionResult Blah(dynamic request)
        {
            return null;
        }

        public override string LoginRedirectUrl
        {
            get { return "~/api/auth"; }
        }
    }
}
