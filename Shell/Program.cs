using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Infrastructure.DI;
using Lending.Execution;
using Lending.Execution.DI;
using Lending.Execution.Persistence;
using Microsoft.Owin.Hosting;
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
        static void Main(string[] args)
        {

            IContainer container = IoC.Initialize<LendingContainer>(new ShellRegistry());

            var url = "http://+:8083";

            var webapp = WebApp.Start<Startup>(url);
            Console.WriteLine("Running on {0}", url);
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
            webapp.Dispose();

            Console.ReadLine();
        }

        private static bool Blah(Assembly assembly)
        {
            return assembly.FullName.StartsWith("Lending");
        }
    }
}
