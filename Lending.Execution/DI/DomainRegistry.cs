using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using EventStore.ClientAPI;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain;
using Lending.Domain.AcceptConnection;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.RegisterUser;
using Lending.Domain.RequestConnection;
using Lending.Execution.Auth;
using Lending.Execution.EventStore;
using Lending.Execution.Nancy;
using Lending.Execution.Persistence;
using Lending.Execution.UnitOfWork;
using Lending.Execution.WebServices;
using Lending.ReadModels.Relational.BookAdded;
using Lending.ReadModels.Relational.SearchForUser;
using Nancy.Authentication.Token;
using Nancy.SimpleAuthentication;
using NHibernate.Context;
//using Nancy;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Memcached;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.ServiceInterface.Auth;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Web;

namespace Lending.Execution.DI
{
    public class DomainRegistry : Registry
    {
        public DomainRegistry()
        {

            var config = Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82
                    .ConnectionString(c => c.FromAppSetting("lender_db"))
                    .DefaultSchema(ConfigurationManager.AppSettings["lender_db_schema"])
                )
                .CurrentSessionContext<ThreadStaticSessionContext>()
                .Mappings(m =>
                    m.FluentMappings
                        .AddFromAssemblyOf<UserAuthPersistenceDto>()
                        .AddFromAssemblyOf<RegisteredUserMap>()
                        .AddFromAssemblyOf<RegisteredUser>()
                        .AddFromAssemblyOf<LibraryBookMap>()
                )
                .BuildConfiguration()
                ;

            For<NHibernate.Cfg.Configuration>()
                .Singleton()
                .Use(config)
                ;

            For<NHibernate.ISessionFactory>()
                .Singleton()
                .Use(config.BuildSessionFactory())
                ;

            var settings = ConfigurationManager.AppSettings;

            For<NHibernate.ISession>()
                .Use(c => c.GetInstance<IUnitOfWork>().CurrentSession)
                ;

            For<IRepository>()
                .AlwaysUnique()
                .Use<NHibernateRepository>()
                ;

            Scan(scanner =>
            {
                scanner.AssemblyContainingType<Command>();
                scanner.AssemblyContainingType<AddBookToLibrary>();
                scanner.AssemblyContainingType<RegisteredUserMap>();
                scanner.AssemblyContainingType<SearchForUser>();
                scanner.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IQueryHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IMessageHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IAuthenticatedMessageHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IAuthenticatedCommandHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(AuthenticatedCommandHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IEventHandler<>));
            });

            For<IUserAuthRepository>()
                .AlwaysUnique()
                .Use<NHibernateUserAuthRepository>()
                ;

            For<IAuthProvider>()
                .AlwaysUnique()
                .Use<UnitOfWorkAuthProvider>()
                ;

            For<AuthService>()
                .AlwaysUnique()
                .Use<UnitOfWorkAuthService>()
                ;

            var cfg = ConfigurationManager.GetSection("enyim.com/memcached") as MemcachedClientSection;
            var cache = new MemcachedClientCache(cfg);

            For<ICacheClient>()
                .Singleton()
                //.Use(cache)
                .Use<MemoryCacheClient>()
                ;

            For<IEventRepository>()
                .AlwaysUnique()
                .Use(c => c.GetInstance<IUnitOfWork>().EventRepository)
                ;

            For<Func<Guid>>()
                .Use(new Func<Guid>(SequentialGuid.NewGuid));
            
            var eventEmitter = new InMemoryEventEmitter();

            For<IEventEmitter>()
                .Singleton()
                .Use(eventEmitter);

            For<InMemoryEventEmitter>()
                .Singleton()
                .Use(eventEmitter);

            For<BlockingCollection<Event>>()
                .Singleton()
                .Use(new BlockingCollection<Event>())
                ;

            For<Func<Type, IEnumerable<IEventHandler>>>()
                .Use<Func<Type, IEnumerable<IEventHandler>>>(context => eventType => GetEventHandlers(context, eventType));

            For<IMessageHandler<SearchForUser, Result>>()
                .AlwaysUnique()
                .Use<SearchForUserHandler>();

            For<EventDispatcher>()
                .AlwaysUnique()
                .Use<EventDispatcher>();

            For<EventHandlerProvider>()
                .AlwaysUnique()
                .Use<EventHandlerProvider>();

            For<IAuthenticationCallbackProvider>()
                .AlwaysUnique()
                .Use<AuthCallbackProvider>();

            For<ITokenizer>()
                .AlwaysUnique()
                .Use(c => new Tokenizer());

        }

        private static IEnumerable<IEventHandler> GetEventHandlers(IContext context, Type eventType)
        {
            Type type = typeof(IEventHandler<>).MakeGenericType(eventType);
            return context.GetAllInstances(type)
                .Select(x => (IEventHandler)x);

        }


    }
}
