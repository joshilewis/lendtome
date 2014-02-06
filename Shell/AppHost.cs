using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core;
using Lending.Core.AddItem;
using Lending.Core.BorrowItem;
using Lending.Core.Connect;
using Lending.Core.Model;
using Lending.Execution.DI;
using Lending.Execution.WebServices;
using Lending.Web;
using ServiceStack;
using ServiceStack.Authentication.OAuth2;
using ServiceStack.Authentication.OpenId;
using ServiceStack.Configuration;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;

namespace Shell
{
    internal class AppHost : AppHostHttpListenerBase
    {
        public AppHost()
            : base("HttpListener Self-Host", typeof(Request).Assembly, typeof(WebserviceBase<,>).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            container.Adapter = new StructureMapContainerAdapter();

            var appSettings = new AppSettings(); 

            Plugins.Add(new AuthFeature(
    () => new AuthUserSession(), //Use your own typed Custom UserSession type
    new IAuthProvider[] {
                    new GoogleOpenIdOAuthProvider(appSettings), //Sign-in with Google OpenId
                    new YahooOpenIdOAuthProvider(appSettings), //Sign-in with Yahoo OpenId
                    new OpenIdOAuthProvider(appSettings), //Sign-in with Custom OpenId
                    new GoogleOAuth2Provider(appSettings), //Sign-in with Google OAuth2 Provider
                    new LinkedInOAuth2Provider(appSettings), //Sign-in with LinkedIn OAuth2 Provider
                }));

            Routes
                .Add<AddUserItemRequest>("/user/{OwnerId}/items/add/", "GET,POST")
                .Add<AddOrganisationItemRequest>("/org/{OwnerId}/items/add/")
                .Add<ConnectRequest>("/connection/add/{FromUserId}/{ToUserId}/")
                .Add<BorrowItemRequest>("/borrow/{OwnershipId}/{RequestorId}/")
                ;
        }
    }

}
