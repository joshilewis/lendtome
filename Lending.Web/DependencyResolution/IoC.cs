using System;
using Lending.Core;
using ServiceStack.ServiceInterface.Auth;
using StructureMap;

namespace Lending.Web.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.LookForRegistries();
                    scan.AssembliesFromApplicationBaseDirectory(a => a.FullName.StartsWith("Lending"));
                    scan.WithDefaultConventions();
                    //scan.TheCallingAssembly();
                });

                x.For<IRequestHandler<IAuthSession, BaseResponse>>()
                    .AlwaysUnique()
                    .Use<FormsAuthNewUserRequestHandler>()
                    ;
            });

            ObjectFactory.AssertConfigurationIsValid();
            string blah = ObjectFactory.WhatDoIHave();

            //new SchemaUpdate(ObjectFactory.GetInstance<Configuration>())
            //    .Execute(true, true);

            return ObjectFactory.Container;
        }
    }
}