using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Lending.Cqrs;
using Lending.Domain;
using Lending.Execution.DI;
using Lending.Execution.EventStore;
using Lending.Execution.UnitOfWork;
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
            IContainer container = IoC.Container;

            container.AssertConfigurationIsValid();

        }
    }

}
