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
    /// https://github.com/joshilewis/lending/issues/9
    /// As a User I want to Add Books to my Library so that my Connections can see what Books I own.
    /// </summary>
    public class AddBookToLibraryTests : FixtureWithEventStoreAndNHibernate
    {

        /// <summary>
        /// GIVEN User1 is a Registered User
        /// WHEN User1 Adds Book1 to her Library
        /// THEN Book1 is Added to User1's Library
        /// </summary>
        [Test]
        public void AddingNewBookToLibraryShouldSucceed()
        {
            Given(Library1Opens);
            When(AddBook1ToLibrary);
            Then(Created);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, Book1AddedToUser1Library);
        }

        /// <summary>
        /// GIVEN User1 is a Registered User AND Book1 is in User1's Library
        /// WHEN User1 Adds Book1 to her Library
        /// THEN User1 is notified that Book1 is already in her Library
        /// </summary>
        [Test]
        public void AddingDuplicateBookToLibraryShouldFail()
        {
            Given(Library1Opens, AddBook1ToLibrary);
            When(AddBook1ToLibrary);
            Then(FailBecauseBookAlreadyInLibrary1);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, Book1AddedToUser1Library);
        }

        /// <summary>
        /// GIVEN User1 is a Registered User AND User1 has Added and Removed Book1 from her Library
        /// WHEN User1 Adds Book1 to her Library
        /// THEN Book1 is Added to User1's Library
        /// </summary>
        [Test]
        public void AddingPreviouslyRemovedBookToLibraryShouldSucceed()
        {
            Given(Library1Opens, AddBook1ToLibrary, User1RemovesBookFromLibrary);
            When(AddBook1ToLibrary);
            Then(Created);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, Book1AddedToUser1Library, Book1RemovedFromLibrary, Book1AddedToUser1Library);
        }

        [Test]
        public void UnauthorizedAddBookAddBookShouldFail()
        {
            Given(Library1Opens);
            When(UnauthorizedAddBookToLibrary);
            Then(FailBecauseUnauthorized(UnauthorizedAddBookToLibrary.UserId,
                UnauthorizedAddBookToLibrary.AggregateId, typeof(Library)));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }

    }
}
