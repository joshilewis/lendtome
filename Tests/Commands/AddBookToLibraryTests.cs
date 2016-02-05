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
            GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            WhenCommand(AddBook1ToLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/add");
            Then(Http201Created);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, Book1AddedToUser1Library);
            AndGETTo<LibraryBook[]>($"/libraries/{Library1Id}/books/").Returns(new[]
            {
                new LibraryBook(processId, OpenedLibrary1, OpenLibrary1.Name, AddBook1ToLibrary.Title,
                    AddBook1ToLibrary.Author, AddBook1ToLibrary.Isbn),
            });
        }

        /// <summary>
        /// GIVEN User1 is a Registered User AND Book1 is in User1's Library
        /// WHEN User1 Adds Book1 to her Library
        /// THEN User1 is notified that Book1 is already in her Library
        /// </summary>
        [Test]
        public void AddingDuplicateBookToLibraryShouldFail()
        {
            GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            GivenCommand(AddBook1ToLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/add");
            WhenCommand(AddBook1ToLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/add");
            Then(Http400Because(Library.BookAlreadyInLibrary));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, Book1AddedToUser1Library);
            AndGETTo<LibraryBook[]>($"/libraries/{Library1Id}/books/").Returns(new[]
            {
                new LibraryBook(processId, OpenedLibrary1, OpenLibrary1.Name, AddBook1ToLibrary.Title,
                    AddBook1ToLibrary.Author, AddBook1ToLibrary.Isbn),
            });
        }

        /// <summary>
        /// GIVEN User1 is a Registered User AND User1 has Added and Removed Book1 from her Library
        /// WHEN User1 Adds Book1 to her Library
        /// THEN Book1 is Added to User1's Library
        /// </summary>
        [Test]
        public void AddingPreviouslyRemovedBookToLibraryShouldSucceed()
        {
            GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            GivenCommand(AddBook1ToLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/add");
            GivenCommand(User1RemovesBookFromLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/remove");
            WhenCommand(AddBook1ToLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/add");
            Then(Http201Created);
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, Book1AddedToUser1Library, Book1RemovedFromLibrary, Book1AddedToUser1Library);
            AndGETTo<LibraryBook[]>($"/libraries/{Library1Id}/books/").Returns(new[]
            {
                new LibraryBook(processId, OpenedLibrary1, OpenLibrary1.Name, AddBook1ToLibrary.Title,
                    AddBook1ToLibrary.Author, AddBook1ToLibrary.Isbn),
            });
        }

        [Test]
        public void UnauthorizedAddBookAddBookShouldFail()
        {
            GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            WhenCommand(UnauthorizedAddBookToLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/add");
            Then(Http403BecauseUnauthorized(UnauthorizedAddBookToLibrary.UserId, Library1Id, typeof (Library)));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
            AndGETTo<LibraryBook[]>($"/libraries/{Library1Id}/books/").Returns(new LibraryBook[] {});
        }

    }
}
