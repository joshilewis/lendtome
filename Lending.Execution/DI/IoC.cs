using StructureMap;
using StructureMap.Graph;

namespace Lending.Execution.DI
{
    public static class IoC
    {
        public static IContainer Container { get; private set; }

        static IoC()
        {
            Container = Initialize();
        }

        private static IContainer Initialize()
        {
            var container = new LendingContainer();

            container.AssertConfigurationIsValid();
            string blah = container.WhatDoIHave();

            //new SchemaUpdate(ObjectFactory.GetInstance<Configuration>())
            //    .Execute(true, true);

            return container;
        }
    }

    public class LendingContainer : Container
    {
        public LendingContainer()
            : base(x =>
            {
                x.Scan(scan =>
                {
                    scan.LookForRegistries();
                    scan.AssembliesFromApplicationBaseDirectory(a => a.FullName.StartsWith("Lending"));
                    scan.AssembliesFromApplicationBaseDirectory(a => a.FullName.StartsWith("Tests"));
                    scan.AssemblyContainingType<DomainRegistry>();
                    scan.WithDefaultConventions();
                    //scan.TheCallingAssembly();
                });

            })
        {
        }
    }
}