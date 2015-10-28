using System;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using StructureMap;
using StructureMap.Graph;

namespace Lending.Web.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
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
    }
}