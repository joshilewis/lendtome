using System;
using Lending.Cqrs.Query;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.Model;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.ReadModels.Relational.BookAdded;
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
        /// GIVEN Library1 is Open and Book1 is Added to Library1
        /// WHEN Book1 is Removed from Library1
        /// THEN HTTP200 is returned 
        /// AND nothing appears in Library1's Books
        /// </summary>
        [Test]
        public void RemoveBookInLibraryShouldSucceed()
        {
            this.GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            this.GivenCommand(AddBook1ToLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/add");
            this.WhenCommand(User1RemovesBookFromLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/remove");
            this.Then(Http200Ok);
            this.AndGETTo($"/libraries/{Library1Id}/books/").Returns(new LibraryBook[] {});
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, Book1AddedToUser1Library, Book1RemovedFromLibrary);
        }

        /// <summary>
        /// GIVEN Library1 is Open and Book1 has not been Added to Library1
        /// WHEN Book1 is Removed from Library1
        /// THEN HTTP400 is returned because Book1 is not in Library1
        /// AND nothing appears in Library1's Books
        /// </summary>
        [Test]
        public void RemoveBookNotInLibraryShouldFail()
        {
            this.GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            this.WhenCommand(User1RemovesBookFromLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/remove");
            this.Then(this.Http400Because(Library.BookNotInLibrary));
            this.AndGETTo($"/libraries/{Library1Id}/books/").Returns(new LibraryBook[] { });
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }

        [Test]
        public void UnauthorizedRemoveBookInLibraryShouldFail()
        {
            this.GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            this.GivenCommand(AddBook1ToLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/add");
            this.WhenCommand(UnauthorizedRemoveBook).IsPOSTedTo($"/libraries/{Library1Id}/books/remove");
            this.Then(this.Http403BecauseUnauthorized(UnauthorizedRemoveBook.UserId, Library1Id, typeof (Library)));
            this.AndGETTo($"/libraries/{Library1Id}/books/").Returns(new LibraryBook[] { });
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, Book1AddedToUser1Library);
        }

    }
}
