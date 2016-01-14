using System;
using Lending.Cqrs.Query;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.Model;
using Lending.Domain.RemoveBookFromLibrary;
using NUnit.Framework;
using static Tests.DefaultTestData;

namespace Tests.Commands
{
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/11
    /// As a User I want to Remove Books from my Collection so that my Connections can see that I no longer own the book.
    /// </summary>
    [TestFixture]
    public class RemoveBookFromLibraryTests : FixtureWithEventStoreAndNHibernate
    {
        /// <summary>
        /// GIVEN 'User1' is a Registered User AND Book 'Book1' is in User1's Library 
        /// WHEN User1 Removes Book1 from her Library
        /// THEN Book1 is Removed from User1's Library
        /// </summary>
        [Test]
        public void RemoveBookInLibraryShouldSucceed()
        {
            Given(Library1Opens, AddBook1ToLibrary);
            When(user1RemovesBookFromLibrary);
            Then(Succeed);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, Book1AddedToUser1Library, book1RemovedFromLibrary);
        }

        /// <summary>
        /// GIVEN 'User1' is a Registered user AND Book1 is not in User1's Library
        /// WHEN User1 Removes Book1 from her Library
        /// THEN User1 is notified that Book1 is not in her Library        
        /// </summary>
        [Test]
        public void RemoveBookNotInLibraryShouldFail()
        {
            Given(Library1Opens);
            When(user1RemovesBookFromLibrary);
            Then(FailBecauseBookNotInLibrary);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }

    }
}
