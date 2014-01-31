using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lending.Core;
using Lending.Core.AddUser;
using Lending.Execution.WebServices;
//using Nancy.Hosting.Self;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using StructureMap;
using Environment = System.Environment;

namespace Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(y =>
                {
                    y.WithDefaultConventions();
                    y.LookForRegistries();
                    //y.AssembliesFromPath(Environment.CurrentDirectory, a => a.FullName.StartsWith("Lending"));
                    y.AssembliesFromPath(Environment.CurrentDirectory, Blah);
                });


            });

            ObjectFactory.AssertConfigurationIsValid();
            string blah = ObjectFactory.WhatDoIHave();

            new SchemaUpdate(ObjectFactory.GetInstance<Configuration>())
                .Execute(true, true);

            //using (var host = new NancyHost(new Uri("http://localhost:8080")))
            //{
            //    host.Start();
            //    Console.WriteLine("Nancy has started");
            //    Console.ReadLine();
            //} 


            var host = new AppHost();
            host.Init();
            host.Start("http://localhost:8085/");
            Console.WriteLine("Listening, GO!");
            Console.ReadLine();
        }

        private static bool Blah(Assembly assembly)
        {
            return assembly.FullName.StartsWith("Lending");
        }
    }
}
