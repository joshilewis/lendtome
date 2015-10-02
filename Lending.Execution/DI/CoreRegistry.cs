using System;
using System.Collections.Concurrent;
using System.Configuration;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Lending.Domain;
using Lending.Execution.Auth;
using Lending.Execution.EventStore;
using Lending.Execution.UnitOfWork;
using Lending.Execution.WebServices;
//using Nancy;
using NHibernate.Context;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Memcached;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.ServiceInterface.Auth;
using StructureMap.Configuration.DSL;
using Configuration = NHibernate.Cfg.Configuration;
using ISession = NHibernate.ISession;
using ISessionFactory = NHibernate.ISessionFactory;
using Request = Lending.Domain.Request;

namespace Lending.Execution.DI
{
    public class CoreRegistry : Registry
    {
        public CoreRegistry()
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
                        .AddFromAssemblyOf<ServiceStackUser>()
                )
                .BuildConfiguration()
                ;

            For<Configuration>()
                .Singleton()
                .Use(config)
                ;

            For<ISessionFactory>()
                .Singleton()
                .Use(config.BuildSessionFactory())
                ;

            For<IUnitOfWork>()
                .HybridHttpOrThreadLocalScoped()
                .Use<UnitOfWork.UnitOfWork>()
                .Ctor<string>()
                .EqualToAppSetting("EventStore:IPAddress")
                ;

            For<ISession>()
                .Use(c => c.GetInstance<IUnitOfWork>().CurrentSession)
                ;

            Scan(scanner =>
            {
                scanner.AssemblyContainingType<Request>();
                scanner.AssemblyContainingType<ServiceStackUser>();
                scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IAuthenticatedRequestHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(BaseAuthenticatedRequestHandler<,>));
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

            For<ConcurrentQueue<Aggregate>>()
                .Use(c => c.GetInstance<IUnitOfWork>().Queue)
                ;

            For<IRepository>()
                .AlwaysUnique()
                .Use<EventStoreRepository>()
                ;

            For<Func<Guid>>()
                .Use(() => SequentialGuid.NewGuid());
        }


    }
}
