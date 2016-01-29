using System;
using System.Collections.Generic;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;

namespace Lending.Execution.DI
{
    public static class IoC
    {
        public static IContainer Container { get; private set; }

        public static IContainer Initialize(params Registry[] registryTypesToInclude)
        {
            Container = new LendingContainer();

            Container.Configure(x =>
            {
                foreach (Registry registryType in registryTypesToInclude)
                {
                    x.IncludeRegistry(registryType);
                }
            });

            string blah = Container.WhatDoIHave();
            string scanned = Container.WhatDidIScan();
            TypeRepository.AssertNoTypeScanningFailures();
            Container.AssertConfigurationIsValid();

            //new SchemaUpdate(ObjectFactory.GetInstance<Configuration>())
            //    .Execute(true, true);

            return Container;
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
                    scan.AssemblyContainingType<DomainRegistry>();
                    scan.WithDefaultConventions();
                    //scan.TheCallingAssembly();
                });

            })
        {
        }
    }
}