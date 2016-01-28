using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Execution.UnitOfWork;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Shell
{
    public class ShellRegistry : Registry
    {
        public ShellRegistry()
        {
            string eventStoreIpAddress = ConfigurationManager.AppSettings["EventStore:IPAddress"];

            For<IUnitOfWork>()
                .Use<UnitOfWork>()
                .Ctor<string>()
                .Is(eventStoreIpAddress);

        }
    }
}
