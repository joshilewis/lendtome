using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Web.Mvc;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Domain;
using Lending.Domain.AcceptConnection;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.Domain.RequestConnection;
using Lending.Execution.Auth;
using Lending.Execution.DI;
using Lending.Execution.UnitOfWork;
using Lending.Execution.WebServices;
using Lending.ReadModels.Relational.SearchForUser;
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
            : base("lend-to.me services host", typeof(AppHost).Assembly, typeof(Command).Assembly, typeof(Webservice<,>).Assembly) { }

	    public override void Configure(Funq.Container container)
	    {
            container.Adapter = new StructureMapContainerAdapter();

            SetConfig(new EndpointHostConfig() { ServiceStackHandlerFactoryPath = "api" });
	        //Set JSON web services to return idiomatic JSON camelCase properties
            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

	        //Configure User Defined REST Paths
	        Routes
                .Add<RequestConnection>("/connections/request/{TargetUserId}/")
                .Add<AcceptConnection>("/connections/accept/{TargetUserId}/")
                .Add<AddBookToLibrary>("/books/add/")
                .Add<RemoveBookFromLibrary>("/books/remove/")
                .Add<SearchForUser>("/users/{SearchString}")
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
                    new GoogleOpenIdOAuthProvider(appSettings),
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