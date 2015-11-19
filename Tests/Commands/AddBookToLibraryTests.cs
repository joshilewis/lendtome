using System;
using Lending.Cqrs.Query;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.Model;
using Lending.Domain.RegisterUser;
using Lending.Domain.RemoveBookFromLibrary;
using NUnit.Framework;

namespace Tests.Commands
{
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/9
    /// As a User I want to Add Books to my Library so that my Connections can see what Books I own.
    /// </summary>
    public class AddBookToLibraryTests : FixtureWithEventStoreAndNHibernate
    {
        private Guid processId = Guid.Empty;
        private Guid user1Id;

        //Commands
        private RegisterUser user1Registers;
        private AddBookToLibrary user1AddsBookToLibrary;
        private RemoveBookFromLibrary user1RemovesBookFromLibrary;

        //Events
        private UserRegistered user1Registered;
        private BookAddedToLibrary book1AddedToUser1Library;
        private BookRemovedFromLibrary book1RemovedFromLibrary;

        //Results
        private readonly Result succeed = new Result();
        private readonly Result failBecauseBookAlreadyInLibrary = new Result(User.BookAlreadyInLibrary);

        public override void SetUp()
        {
            base.SetUp();
            user1Id = Guid.NewGuid();
            processId = Guid.NewGuid();
            user1Registers = new RegisterUser(processId, user1Id, 1, "user1", "email1");
            user1Registered = new UserRegistered(processId, user1Id, 1, user1Registers.UserName,
                user1Registers.PrimaryEmail);
            var title = "title";
            var author = "author";
            var isbnnumber = "isbn";
            user1AddsBookToLibrary = new AddBookToLibrary(processId, user1Id, user1Id, title, author, isbnnumber);
            book1AddedToUser1Library = new BookAddedToLibrary(processId, user1Id, title, author, isbnnumber);
            user1RemovesBookFromLibrary = new RemoveBookFromLibrary(processId, user1Id, user1Id, title, author,
                isbnnumber);
            book1RemovedFromLibrary = new BookRemovedFromLibrary(processId, user1Id, title, author, isbnnumber);
        }

        /// <summary>
        /// GIVEN User1 is a Registered User
        /// WHEN User1 Adds Book1 to her Library
        /// THEN Book1 is Added to User1's Library
        /// </summary>
        [Test]
        public void AddingNewBookToLibraryShouldSucceed()
        {
            Given(user1Registers);
            When(user1AddsBookToLibrary);
            Then(succeed);
            AndEventsSavedForAggregate<User>(user1Id, user1Registered, book1AddedToUser1Library);
        }

        /// <summary>
        /// GIVEN User1 is a Registered User AND Book1 is in User1's Library
        /// WHEN User1 Adds Book1 to her Library
        /// THEN User1 is notified that Book1 is already in her Library
        /// </summary>
        [Test]
        public void AddingDuplicateBookToLibraryShouldFail()
        {
            Given(user1Registers, user1AddsBookToLibrary);
            When(user1AddsBookToLibrary);
            Then(failBecauseBookAlreadyInLibrary);
            AndEventsSavedForAggregate<User>(user1Id, user1Registered, book1AddedToUser1Library);
        }

        /// <summary>
        /// GIVEN User1 is a Registered User AND User1 has Added and Removed Book1 from her Library
        /// WHEN User1 Adds Book1 to her Library
        /// THEN Book1 is Added to User1's Library
        /// </summary>
        [Test]
        public void AddingPreviouslyRemovedBookToLibraryShouldSucceed()
        {
            Given(user1Registers, user1AddsBookToLibrary, user1RemovesBookFromLibrary);
            When(user1AddsBookToLibrary);
            Then(succeed);
            AndEventsSavedForAggregate<User>(user1Id, user1Registered, book1AddedToUser1Library, book1RemovedFromLibrary, book1AddedToUser1Library);
        }

    }
}
