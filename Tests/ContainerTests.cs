using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Joshilewis.Infrastructure.DI;
using Lending.Domain;
using Lending.Execution.DI;
using Lending.Web.DependencyResolution;
using NUnit.Framework;
using StructureMap;

namespace Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void Test()
        {
            IContainer container = IoC.Initialize<LendingContainer>(new WebRegistry());

            container.AssertConfigurationIsValid();

        }
    }

}
