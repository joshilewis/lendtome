using System.Configuration;
using Lending.Execution.UnitOfWork;
using StructureMap;

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
