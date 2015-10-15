using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests.Domain
{
    public class AddItemToCollectionHandlerTests : FixtureWithEventStoreAndNHibernate
    {
        [Test]
        public void AddingNonExistentItemToCollectionShouldCreateItemAndAddToCollection()
        {

            Assert.Fail();
        }

        [Test]
        public void AddingNewExistingItemToCollectionShouldAddToCollection()
        {

            Assert.Fail();
        }

        [Test]
        public void AddingDuplicateExistingItemToCollectionShouldFail()
        {

            Assert.Fail();
        }

    }
}
