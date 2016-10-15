using System;
using Joshilewis.Infrastructure;
using Lending.Execution.DI;
using NUnit.Framework;
using static Joshilewis.Testing.Helpers.DIExtensions;
using static Joshilewis.Testing.Helpers.ApiExtensions;
using static Joshilewis.Testing.Helpers.EventStoreExtensions;
using static Tests.MigrationExtensions;

namespace Tests
{
    [TestFixture]
    public abstract class Fixture
    {
        [SetUp]
        public virtual void SetUp()
        {
            SetUpDependencyProvision<LendingContainer>(new TestRegistry());
            SetUpOwinServer<Startup>();
            SetUpEventStore();
            BuildSchema();
        }

        [TearDown]
        public virtual void TearDown()
        {
            DropSchema();
            TearDownEventStore();
            TearDownOwinServer();
        }

        protected virtual void Given(Action action)
        {
            action();
        }

        protected virtual void When(Action action)
        {
            action();
        }

        protected virtual void Then(Action action)
        {
            action();
        }

    }


}
