using System;
using System.Collections.Generic;
using Joshilewis.Cqrs;
using Joshilewis.Infrastructure.UnitOfWork;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Joshilewis.Testing.Helpers
{
    public static class PersistenceExtensions
    {
        private static Configuration Configuration => DIExtensions.Container.GetInstance<Configuration>();
        public static IRepository Repository => DIExtensions.Container.GetInstance<IRepository>();
        public static ISession Session => DIExtensions.Container.GetInstance<ISession>();

        public static void SetUpPersistence()
        {
            new SchemaExport(Configuration)
                .Execute(true, true, false);
        }

        public static void TearDownPersistence()
        {
            //Tear down DB
            new SchemaExport(Configuration)
                .Execute(false, true, true);

        }

        public static void OpenTransaction()
        {
            DIExtensions.Container.GetInstance<IUnitOfWork>().Begin();
        }

        public static void CommitTransaction()
        {
            DIExtensions.Container.GetInstance<IUnitOfWork>().Commit();
        }

        public static void SaveEntities(params object[] entitiesToSave)
        {
            OpenTransaction();
            foreach (var entity in entitiesToSave)
            {
                Repository.Save(entity);
            }
            CommitTransaction();
        }


    }
}
