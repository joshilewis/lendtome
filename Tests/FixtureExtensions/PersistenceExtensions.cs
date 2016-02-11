using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Execution.UnitOfWork;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using static Tests.FixtureExtensions.DIExtensions;

namespace Tests.FixtureExtensions
{
    public static class PersistenceExtensions
    {
        private static Configuration Configuration => Container.GetInstance<Configuration>();
        private static IRepository Repository => Container.GetInstance<IRepository>();
        public static ISession Session => Container.GetInstance<ISession>();

        public static void SetUpPersistence()
        {
            new SchemaExport(Configuration)
                .Execute(true, true, false);

            Container.GetInstance<IUnitOfWork>().Begin();
        }

        public static void TearDownPersistence()
        {
            CommitTransaction();

            //Tear down DB
            new SchemaExport(Configuration)
                .Execute(false, true, true);

        }

        public static void CommitTransaction()
        {
            Container.GetInstance<IUnitOfWork>().Commit();
        }

        public static void SaveEntities(params object[] entitiesToSave)
        {
            foreach (var entity in entitiesToSave)
            {
                Repository.Save(entity);
            }
        }


    }
}
