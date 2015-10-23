using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests.Domain
{
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/11
    /// As a User I want to Remove Books from my Collection so that my Connections can see that I no longer own the book.
    /// </summary>
    [TestFixture]
    public class RemoveBookFromCollectionHandlerTests : FixtureWithEventStore
    {
        /// <summary>
        /// GIVEN User 'User1' is a registered user AND Book 'Book1' is in User1's Collection 
        /// WHEN User1 Removes Book1 from her Collection
        /// THEN Book1 is removed from User1's Collection
        /// </summary>
        [Test]
        public void RemoveBookInCollectionShouldSucceed()
        {
            Assert.Fail();
        }

        /// <summary>
        /// GIVEN User 'User1' is a registered user AND Book1 is not in User1's Collection
        ///WHEN User1 Removes Book1 from her Collection
        ///THEN The user is notified that Book1 is not in her Collection
        /// </summary>
        [Test]
        public void RemoveBookNotInCollectionShouldFail()
        {
            Assert.Fail();
        }

    }
}
