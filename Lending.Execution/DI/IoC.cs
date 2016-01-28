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
            var container = new Container(x =>
            {

                x.Scan(scan =>
                {
                    scan.LookForRegistries();
                    scan.AssembliesFromApplicationBaseDirectory(a => a.FullName.StartsWith("Lending"));
                    scan.WithDefaultConventions();
                    //scan.TheCallingAssembly();
                });

            });

            container.AssertConfigurationIsValid();
            string blah = container.WhatDoIHave();

            //new SchemaUpdate(ObjectFactory.GetInstance<Configuration>())
            //    .Execute(true, true);

            return container;
        }
    }
}