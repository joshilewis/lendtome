using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors.Postgres;

namespace Tests
{
    public static class MigrationExtensions
    {
        private static readonly MigrationRunner runner;

        static MigrationExtensions()
        {
            var announcer = new TextWriterAnnouncer(Console.WriteLine);

            Assembly assembly = Assembly.GetExecutingAssembly();

            var migrationContext = new RunnerContext(announcer);

            var options = new MigrationOptions {PreviewOnly = false, Timeout = 60};
            PostgresProcessorFactory factory = new PostgresProcessorFactory();
            string connectionString = ConfigurationManager.ConnectionStrings["lender_db"].ConnectionString;
            IMigrationProcessor processor = factory.Create(connectionString, announcer, options);
            runner = new MigrationRunner(assembly, migrationContext, processor);

        }

        public static void BuildSchema()
        {
            runner.MigrateUp(true);
        }

        public static void DropSchema()
        {
            runner.MigrateDown(0);
        }
    }
    public class MigrationOptions : IMigrationProcessorOptions
    {
        public bool PreviewOnly { get; set; }
        public string ProviderSwitches { get; set; }
        public int Timeout { get; set; }
    }
}
