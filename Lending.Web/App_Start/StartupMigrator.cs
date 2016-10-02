using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Infrastructure.Migrations;
using Lending.ReadModels.Relational.Migrations;

namespace Lending.Web
{
    public class StartupMigrator
    {
        public void InitialiseAndMigrate()
        {
            new Migrator(ConfigurationManager.AppSettings["lender_db"],
                typeof(InitialCreation).Assembly).BuildSchema();
        }
    }
}
