using System.Configuration;
using Joshilewis.Infrastructure.UnitOfWork;
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
