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
        private static IRepository Repository => DIExtensions.Container.GetInstance<IRepository>();
        public static ISession Session => DIExtensions.Container.GetInstance<ISession>();

        public static void SetUpPersistence()
        {
            new SchemaExport(Configuration)
                .Execute(true, true, false);

            DIExtensions.Container.GetInstance<IUnitOfWork>().Begin();
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
            DIExtensions.Container.GetInstance<IUnitOfWork>().Commit();
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
