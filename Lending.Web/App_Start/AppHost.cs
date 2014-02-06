using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Web.Mvc;
using Lending.Core;
using Lending.Core.AddItem;
using Lending.Core.BorrowItem;
using Lending.Core.Connect;
using Lending.Execution.Auth;
using Lending.Execution.DI;
using Lending.Execution.UnitOfWork;
using Lending.Execution.WebServices;
using ServiceStack.Authentication.OAuth2;
using ServiceStack.Authentication.OpenId;
using ServiceStack.Configuration;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Mvc;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.WebHost.Endpoints;

//IMPORTANT: Add the line below to MvcApplication.RegisterRoutes(RouteCollection) in the Global.asax:
//routes.IgnoreRoute("api/{*pathInfo}"); 

/**
 * Entire ServiceStack Starter Template configured with a 'Hello' Web Service and a 'Todo' Rest Service.
 *
 * Auto-Generated Metadata API page at: /metadata
 * See other complete web service examples at: https://github.com/ServiceStack/ServiceStack.Examples
 */

namespace Lending.Web.App_Start
{
	//A customizeable typed UserSession that can be extended with your own properties
	//To access ServiceStack's Session, Cache, etc from MVC Controllers inherit from ControllerBase<CustomUserSession>
	public class CustomUserSession : AuthUserSession
	{
		public string CustomProperty { get; set; }
	}

	public class AppHost
		: AppHostBase
	{		
		public AppHost() //Tell ServiceStack the name and where to find your web services
            : base("StarterTemplate ASP.NET Host", typeof(AppHost).Assembly, typeof(Request).Assembly, typeof(WebserviceBase<,>).Assembly) { }

	    public override void Configure(Funq.Container container)
	    {
            container.Adapter = new StructureMapContainerAdapter();

            SetConfig(new EndpointHostConfig() { ServiceStackHandlerFactoryPath = "api" });
	        //Set JSON web services to return idiomatic JSON camelCase properties
            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

	        //Configure User Defined REST Paths
	        Routes
                .Add<AddUserItemRequest>("/user/{OwnerId}/items/add/", "GET,POST")
                .Add<AddOrganisationItemRequest>("/org/{OwnerId}/items/add/")
                .Add<ConnectRequest>("/connection/add/{FromUserId}/{ToUserId}/")
                .Add<BorrowItemRequest>("/borrow/{OwnershipId}/{RequestorId}/")
                //.Add(typeof(object), "/authed/", "GET,POST")
                ;

	        //Enable Authentication
	        ConfigureAuth(container);

	    }

        private void ConfigureAuth(Funq.Container container)
        {
            var appSettings = new AppSettings();

            //Default route: /auth/{provider}
            Plugins.Add( new AuthFeature(
                () => new AuthUserSession(), //Use your own typed Custom UserSession type
                new IAuthProvider[]
                {
                    new GoogleOpenIdOAuthProvider(appSettings), //Sign-in with Google OpenId
                    new YahooOpenIdOAuthProvider(appSettings), //Sign-in with Yahoo OpenId
                    //new OpenIdOAuthProvider(appSettings), //Sign-in with Custom OpenId
                    //new GoogleOAuth2Provider(appSettings), //Sign-in with Google OAuth2 Provider
                    //new LinkedInOAuth2Provider(appSettings), //Sign-in with LinkedIn OAuth2 Provider
                    container.Adapter.Resolve<IAuthProvider>(),

                }));

            //Default route: /register
            //Plugins.Add(new RegistrationFeature()); 

        }

        public static void Start()
		{
			new AppHost().Init();
		}
	}
}