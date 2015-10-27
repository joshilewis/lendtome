using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lending.Execution.DI;
using Lending.Execution.Persistence;
using Lending.Execution.WebServices;
//using Nancy.Hosting.Self;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using StructureMap;
using StructureMap.Graph;
using Environment = System.Environment;

namespace Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(x =>
            {
                x.Scan(y =>
                {
                    y.WithDefaultConventions();
                    y.LookForRegistries();
                    //y.AssembliesFromPath(Environment.CurrentDirectory, a => a.FullName.StartsWith("Lending"));
                    y.AssembliesFromPath(Environment.CurrentDirectory, Blah);
                });


            });

            container.AssertConfigurationIsValid();
            string blah = container.WhatDoIHave();

            SchemaUpdater.UpdateSchema();

            //using (var host = new NancyHost(new Uri("http://localhost:8080")))
            //{
            //    host.Start();
            //    Console.WriteLine("Nancy has started");
            //    Console.ReadLine();
            //} 


            var host = new AppHost(new StructureMapContainerAdapter(container));
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
