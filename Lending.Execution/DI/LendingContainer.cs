using StructureMap;
using StructureMap.Graph;

namespace Lending.Execution.DI
{
    public class LendingContainer : Container
    {
        public LendingContainer()
            : base(x =>
            {
                x.Scan(scan =>
                {
                    scan.LookForRegistries();
                    scan.AssembliesFromApplicationBaseDirectory(a => a.FullName.StartsWith("Lending"));
                    scan.AssemblyContainingType<DomainRegistry>();
                    scan.WithDefaultConventions();
                    //scan.TheCallingAssembly();
                });

            })
        {
        }
    }
}