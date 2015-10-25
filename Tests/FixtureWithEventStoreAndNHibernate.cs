using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Lending.Cqrs;
using Lending.Domain.RegisterUser;
using Lending.Execution.Auth;
using Lending.Execution.Persistence;
using NCrunch.Framework;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using ServiceStack.Authentication.NHibernate;
using Configuration = NHibernate.Cfg.Configuration;

namespace Tests
{
    [ExclusivelyUses("Database")]
    [NUnit.Framework.Category("Persistence")]
    [TestFixture]
    public abstract class FixtureWithEventStoreAndNHibernate : FixtureWithEventStore
    {
        protected static readonly Configuration Configuration;
        protected static readonly ISessionFactory SessionFactory;
        protected IRepository Repository;
        protected ISession Session;

        static FixtureWithEventStoreAndNHibernate()
        {
            //Set up database
            Configuration = Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82
                    .ConnectionString(c => c.FromConnectionStringWithKey("lender_db"))
                    .DefaultSchema(ConfigurationManager.AppSettings["lender_db_schema"])
                    )
                .Mappings(m =>
                    m.FluentMappings
                        .AddFromAssemblyOf<ServiceStackUser>()
                        .AddFromAssemblyOf<UserAuthPersistenceDto>()
                        .AddFromAssemblyOf<RegisteredUser>()
                )
                .BuildConfiguration();

            SessionFactory = Configuration.BuildSessionFactory();
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            //Create DB
            new SchemaExport(Configuration)
                .Execute(true, true, false);

            Session = SessionFactory.OpenSession();
            Session.BeginTransaction();
            Repository = new NHibernateRepository(() => Session);

        }

        [TearDown]
        public override void TearDown()
        {
            CommitTransaction();

            //Tear down DB
            new SchemaExport(Configuration)
                .Execute(false, true, true);

            base.TearDown();
        }

        protected void CommitTransaction()
        {
            Session.Transaction.Commit();
            Session.Flush();
            Session.Transaction.Dispose();
            Session.Dispose();

        }

        protected void CommitTransactionAndOpenNew()
        {
            CommitTransaction();

            Session = SessionFactory.OpenSession();
            Session.BeginTransaction();

        }

        protected void SaveEntities(params object[] entitiesToSave)
        {
            foreach (var entity in entitiesToSave)
            {
                Repository.Save(entity);
            }
        }
    }
}
