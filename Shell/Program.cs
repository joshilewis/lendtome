using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core;
using Lending.Core.AddUser;
using Lending.Execution.WebServices;
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
                    y.AssembliesFromPath(Environment.CurrentDirectory, a => a.FullName.StartsWith("Lending"));
                });


            });

            ObjectFactory.AssertConfigurationIsValid();
            string blah = ObjectFactory.WhatDoIHave();

            new SchemaUpdate(ObjectFactory.GetInstance<Configuration>())
                .Execute(true, true);

            var webService = ObjectFactory.GetInstance<WebserviceBase<AddUserRequest, BaseResponse>>();

            var response = webService.Execute(new AddUserRequest(){UserName = "joshilewis", EmailAddress = "emailaddress"});

            
        }
    }
}
