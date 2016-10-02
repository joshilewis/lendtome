using System.Configuration;
using Joshilewis.Infrastructure.Migrations;
using Lending.ReadModels.Relational.Migrations;

namespace Tests
{
    public static class MigrationExtensions
    {
        private static readonly Migrator Migrator;

        static MigrationExtensions()
        {
            Migrator = new Migrator(
                ConfigurationManager.ConnectionStrings["lender_db"].ConnectionString,
                typeof(InitialCreation).Assembly);
        }

        public static void BuildSchema()
        {
            Migrator.BuildSchema();
        }

        public static void DropSchema()
        {
            Migrator.DropSchema();
        }
    }
}
