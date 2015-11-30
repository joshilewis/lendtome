using System.Collections.Generic;
using System.Configuration;
using System.Net;
using EventStore.ClientAPI;
using Lending.Execution.UnitOfWork;
using SimpleAuthentication.Core;
using SimpleAuthentication.Core.Providers;
using StructureMap;
using StructureMap.Graph;

namespace Shell
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            var container = new Container(x =>
            {
                string eventStoreIpAddress = ConfigurationManager.AppSettings["EventStore:IPAddress"];

                x.For<IUnitOfWork>()
                    //.
                    .Use<UnitOfWork>()
                    .Ctor<string>()
                    .Is(eventStoreIpAddress);

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