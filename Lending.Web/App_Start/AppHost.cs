using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Web.Mvc;
using Lending.Core;
using Lending.Core.AddItem;
using Lending.Core.BorrowItem;
using Lending.Core.Connect;
using Lending.Core.GetUserItems;
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
            : base("lend-to.me services host", typeof(AppHost).Assembly, typeof(Request).Assembly, typeof(WebserviceBase<,>).Assembly) { }

	    public override void Configure(Funq.Container container)
	    {
            container.Adapter = new StructureMapContainerAdapter();

            SetConfig(new EndpointHostConfig() { ServiceStackHandlerFactoryPath = "api" });
	        //Set JSON web services to return idiomatic JSON camelCase properties
            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

	        //Configure User Defined REST Paths
	        Routes
                .Add<GetUserItemsRequest>("/user/items/", "GET")
                .Add<AddUserItemRequest>("/user/items/add/", "GET,POST")
                .Add<AddOrganisationItemRequest>("/org/{OwnerId}/items/add/")
                .Add<ConnectRequest>("/connection/add/{FromUserId}/{ToUserId}/")
                .Add<BorrowItemRequest>("/borrow/{OwnershipId}/{RequestorId}/")
                ;

	        //Enable Authentication
	        ConfigureAuth(container);
            Plugins.Add(new SessionFeature());
	    }

        private void ConfigureAuth(Funq.Container container)
        {
            var appSettings = new AppSettings();

            //Default route: /auth/{provider}
            Plugins.Add(new AuthFeature(
                () => new AuthUserSession(), //Use your own typed Custom UserSession type
                new IAuthProvider[]
                {
                    new LinkedInOAuth2Provider(appSettings), 
                    new GoogleOAuth2Provider(appSettings),
                    new FacebookAuthProvider(appSettings),
                    new TwitterAuthProvider(appSettings),
                    container.Adapter.Resolve<IAuthProvider>(),

                }, 
                "/api/auth"));

            //Default route: /register
            //Plugins.Add(new RegistrationFeature()); 

        }

        public static void Start()
		{
			new AppHost().Init();
		}
	}
}