using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Domain;
using Lending.Execution.DI;
using NUnit.Framework;
using ServiceStack.ServiceInterface.Auth;
using StructureMap;

namespace Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void Test()
        {
            var container = new Container(x =>
            {
                x.Scan(scan =>
                {
                    scan.LookForRegistries();
                    scan.AssemblyContainingType<DomainRegistry>();
                    scan.WithDefaultConventions();
                });

            });

            container.AssertConfigurationIsValid();

        }
    }
}
