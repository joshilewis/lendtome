 a﻿using System;
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
            GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            WhenCommand(AddBook1ToLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/add");
            Then(Http201Created);
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
            GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            GivenCommand(AddBook1ToLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/add");
            WhenCommand(AddBook1ToLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/add");
            Then(Http400Because(Library.BookAlreadyInLibrary));
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
            // Are you "OPENING" the library? Or loading the information?
            GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            
            // I don't like the command name "Add book to Library" What is the reason for the adding? Is the
            // user returning the book? Is this a donation? Is it a order fulfilled by a purchase order?
            GivenCommand(AddBook1ToLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/add");
            
            // Save deal here. Is the user checking the book out? Or trashing the book?
            GivenCommand(User1RemovesBookFromLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/remove");
            WhenCommand(AddBook1ToLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/add");
            Then(Http201Created);
            
            // Same deal with the events :) You're domain language seems to be dealing in CRUD operations.
            // If your UL is in CRUD, DDD (especially DDD+CQRS+ES) is not a great fit.
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened, Book1AddedToUser1Library, Book1RemovedFromLibrary, Book1AddedToUser1Library);
            
            // Overall, the tests are fine. They test your REST API. Which is great. But I think
            // there is some value in taking the time to properly map out your domain events. As I
            // said on Twitter, the RESTfulness of your API is largely irrelevant. You could do all the
            // IO over POSTs to the same endpoint and just inspect the metadata to route the commands
            // to the correct handler. Doesn't matter. (well, you'll struggle to scale, but whatever)
            // HOW you get the commands from the client to the server is an implementation detail, defining
            // the commands correctly and identifying your domain boundaries is way more important.
            // I find the hardest part is actually identifying the boundaries. And if you really want
            // to have some element of RESTfulness, you would probably do well having an endpoint
            // per boundary. That way you can scale each out independently and all the routing
            // is contained within the same boundary.
            // But step one is to correctly identify those boundaries. Not an easy problem.
        }

        [Test]
        public void UnauthorizedAddBookAddBookShouldFail()
        {
            GivenCommand(OpenLibrary1).IsPOSTedTo("/libraries");
            WhenCommand(UnauthorizedAddBookToLibrary).IsPOSTedTo($"/libraries/{Library1Id}/books/add");
            Then(Http403BecauseUnauthorized(UnauthorizedAddBookToLibrary.UserId, Library1Id, typeof (Library)));
            AndEventsSavedForAggregate<Library>(Library1Id, Library1Opened);
        }

    }
}
