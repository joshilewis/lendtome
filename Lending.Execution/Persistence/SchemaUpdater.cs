using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using StructureMap;

namespace Lending.Execution.Persistence
{
    public class SchemaUpdater
    {
        public static void UpdateSchema()
        {
            new SchemaUpdate(ObjectFactory.GetInstance<Configuration>())
                .Execute(true, true);
        }
    }
}
