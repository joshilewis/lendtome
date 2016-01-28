using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lending.Execution.DI;
using Lending.Execution.Persistence;
using Nancy.Hosting.Self;
//using Nancy.Hosting.Self;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using SimpleAuthentication.Core.Providers;
using StructureMap;
using StructureMap.Graph;
using Environment = System.Environment;

namespace Shell
{
    class Program
    {
        public static IContainer Container;

        static void Main(string[] args)
        {
            Container = IoC.Container;

            //SchemaUpdater.UpdateSchema();

            using (var host = new NancyHost(new Uri("http://localhost:1234")))
            {
                host.Start();
                Console.WriteLine("Listening, GO!");
                Console.ReadLine();
            }


            Console.ReadLine();
        }

        private static bool Blah(Assembly assembly)
        {
            return assembly.FullName.StartsWith("Lending");
        }
    }
}
