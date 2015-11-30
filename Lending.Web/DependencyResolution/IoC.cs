using System;
using System.Configuration;
using System.Net;
using EventStore.ClientAPI;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Execution.Auth;
using Lending.Execution.UnitOfWork;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Web;

namespace Lending.Web.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            var container = new Container(x =>
            {
                string eventStoreIpAddress = ConfigurationManager.AppSettings["EventStore:IPAddress"];

                x.For<IUnitOfWork>()
                    .HybridHttpOrThreadLocalScoped()
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

                //x.For<ICommandHandler<IAuthSession, Result>>()
                //    .AlwaysUnique()
                //    .Use<FormsAuthRegisterUserHandler>()
                //    ;

            });

            container.AssertConfigurationIsValid();
            string blah = container.WhatDoIHave();

            //new SchemaUpdate(ObjectFactory.GetInstance<Configuration>())
            //    .Execute(true, true);

            return container;
        }

        private static IEventStoreConnection Create(string eventStoreIpAddress)
        {
            return EventStoreConnection.Create(new IPEndPoint(IPAddress.Parse(eventStoreIpAddress), 1113));
        }
    }
}