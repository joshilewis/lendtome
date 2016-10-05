using System;
using System.Reflection;
using FluentMigrator;
using FluentMigrator.Infrastructure;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors.Postgres;

namespace Joshilewis.Infrastructure.Migrations
{
    public class Migrator
    {
        private readonly MigrationRunner runner;

        public Migrator(string connectionString, params Assembly[] assemblies)
        {
            var announcer = new TextWriterAnnouncer(Console.Write);

            IAssemblyCollection assemblyCollection = new AssemblyCollection(assemblies);

            var migrationContext = new RunnerContext(announcer);

            var options = new MigrationOptions { PreviewOnly = false, Timeout = 60 };
            PostgresProcessorFactory factory = new PostgresProcessorFactory();

            IMigrationProcessor processor = factory.Create(connectionString, announcer, options);
            runner = new MigrationRunner(assemblyCollection, migrationContext, processor);
        }

        public void BuildSchema()
        {
            runner.MigrateUp(true);
        }

        public void DropSchema()
        {
            runner.MigrateDown(0);
        }
    }
}