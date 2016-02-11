using Joshilewis.Infrastructure.DI;
using Lending.Execution.DI;
using Lending.Web.DependencyResolution;
using NUnit.Framework;
using StructureMap;

namespace Tests.Infra
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
