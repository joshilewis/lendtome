using StructureMap;
using StructureMap.Graph.Scanning;

namespace Joshilewis.Infrastructure.DI
{
    public static class IoC
    {
        public static IContainer Container { get; private set; }

        public static IContainer Initialize<TContainer>(params Registry[] registryTypesToInclude) where TContainer : Container, new()
        {
            Container = new TContainer();

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

            return Container;
        }
    }
}