using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Lending.Core;
using Lending.Core.AddItem;
using Lending.Core.Model;
using Lending.Core.Model.Maps;
using Lending.Execution.UnitOfWork;
using Lending.Execution.WebServices;
//using Nancy;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using StructureMap.Configuration.DSL;
using Request = Lending.Core.Request;

namespace Lending.Execution.DI
{
    public class CoreRegistry : Registry
    {
        public CoreRegistry()
        {

            var config = Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82
                    .ConnectionString(c => c.FromConnectionStringWithKey("lender_db")))
                .CurrentSessionContext<ThreadStaticSessionContext>()
                .Mappings(m =>
                    m.FluentMappings
                        .AddFromAssemblyOf<UserMap>())
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
                ;

            For<ISession>()
                .Use(c => c.GetInstance<IUnitOfWork>().CurrentSession)
                ;

            Scan(scanner =>
            {
                scanner.AssemblyContainingType<Request>();
                scanner.ConnectImplementationsToTypesClosing(typeof (IRequestHandler<,>));
            });

            For<IRequestHandler<AddUserItemRequest, BaseResponse>>()
                .Use<AddItemRequestHandler<User>>()
                ;

            For<IRequestHandler<AddOrganisationItemRequest, BaseResponse>>()
                .Use<AddItemRequestHandler<Organisation>>()
                ;

        }

    }
}
