using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.Model;
using Lending.Domain.RegisterUser;
using Lending.Domain.RemoveBookFromLibrary;
using NUnit.Framework;
using ServiceStack.Text;

namespace Tests.Domain
{
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/11
    /// As a User I want to Remove Books from my Collection so that my Connections can see that I no longer own the book.
    /// </summary>
    [TestFixture]
    public class RemoveBookFromLibraryTests : FixtureWithEventStoreAndNHibernate
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
        private readonly Result failBecauseBookNotInLibrary = new Result(User.BookNotInLibrary);

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
        /// GIVEN 'User1' is a Registered User AND Book 'Book1' is in User1's Library 
        /// WHEN User1 Removes Book1 from her Library
        /// THEN Book1 is Removed from User1's Library
        /// </summary>
        [Test]
        public void RemoveBookInLibraryShouldSucceed()
        {
            Given(user1Registers, user1AddsBookToLibrary);
            When(user1RemovesBookFromLibrary);
            Then(succeed);
            AndEventsSavedForAggregate<User>(user1Id, user1Registered, book1AddedToUser1Library, book1RemovedFromLibrary);
        }

        /// <summary>
        /// GIVEN 'User1' is a Registered user AND Book1 is not in User1's Library
        /// WHEN User1 Removes Book1 from her Library
        /// THEN User1 is notified that Book1 is not in her Library        
        /// </summary>
        [Test]
        public void RemoveBookNotInLibraryShouldFail()
        {
            Given(user1Registers);
            When(user1RemovesBookFromLibrary);
            Then(failBecauseBookNotInLibrary);
            AndEventsSavedForAggregate<User>(user1Id, user1Registered);
        }

    }
}
