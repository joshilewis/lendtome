using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.AddBookToLibrary;
using Lending.Execution.Auth;
using Lending.Execution.Persistence;
using Lending.Execution.UnitOfWork;
using Lending.ReadModels.Relational.BookAdded;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.SearchForLibrary;
using Nancy.SimpleAuthentication;
using NHibernate.Context;
using StructureMap;

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
                        .AddFromAssemblyOf<OpenedLibraryMap>()
                        .AddFromAssemblyOf<LibraryBookMap>()
                        .AddFromAssemblyOf<AuthenticatedUserMap>()
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
                scanner.AssemblyContainingType<SearchForLibrary>();
                scanner.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IQueryHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IMessageHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IAuthenticatedMessageHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IAuthenticatedCommandHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(AuthenticatedCommandHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IEventHandler<>));
            });

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

            For<IMessageHandler<SearchForLibrary, Result>>()
                .AlwaysUnique()
                .Use<SearchForLibraryHandler>();

            For<EventDispatcher>()
                .AlwaysUnique()
                .Use<EventDispatcher>();

            For<EventHandlerProvider>()
                .AlwaysUnique()
                .Use<EventHandlerProvider>();

            string jwtSecret = ConfigurationManager.AppSettings["jwt_secret"];

            For<Tokeniser>()
                .AlwaysUnique()
                .Use<Tokeniser>()
                .Ctor<string>()
                .Is(jwtSecret);

            For<IAuthenticationCallbackProvider>()
                .AlwaysUnique()
                .Use<AuthCallbackProvider>();

            For<IUserMapper>()
                .AlwaysUnique()
                .Use<UserMapper>();

        }

        private static IEnumerable<IEventHandler> GetEventHandlers(IContext context, Type eventType)
        {
            Type type = typeof(IEventHandler<>).MakeGenericType(eventType);
            return context.GetAllInstances(type)
                .Select(x => (IEventHandler)x);

        }


    }
}
