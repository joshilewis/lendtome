using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Maps;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public abstract class DatabaseFixtureBase
    {
        protected static readonly Configuration Configuration;
        protected static readonly ISessionFactory SessionFactory;
        protected ISession Session;

        static DatabaseFixtureBase()
        {
            //Set up database
            Configuration = Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82
                    .ConnectionString(c => c.FromConnectionStringWithKey("lender_db")))
                .Mappings(m =>
                    m.FluentMappings
                        .AddFromAssemblyOf<UserMap>())
                .BuildConfiguration();

            SessionFactory = Configuration.BuildSessionFactory();
        }

        [SetUp]
        public virtual void SetUp()
        {
            //Create DB
            new SchemaExport(Configuration)
                .Execute(true, true, false);

            Session = SessionFactory.OpenSession();
            Session.BeginTransaction();

        }

        [TearDown]
        public virtual void TearDown()
        {
            CommitTransaction();

            //Tear down DB
            new SchemaExport(Configuration)
                .Execute(true, true, true);

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
                Session.Save(entity);
            }
        }
    }
}
