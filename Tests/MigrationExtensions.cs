using System.Configuration;
using Joshilewis.Infrastructure.Migrations;
using Lending.ReadModels.Relational.Migrations;
using Npgsql.Logging;

namespace Tests
{
    public static class MigrationExtensions
    {
        private static Migrator instance;

        private static Migrator Instance
        {
            get
            {
                if (instance== null)
                {
                    instance = new Migrator(
                        ConfigurationManager.ConnectionStrings["lender_db"].ConnectionString,
                        typeof(InitialCreation).Assembly);
                }
                return instance;
            }
        }

        public static void BuildSchema()
        {
            Instance.BuildSchema();
        }

        public static void DropSchema()
        {
            Instance.DropSchema();
        }
    }
}
