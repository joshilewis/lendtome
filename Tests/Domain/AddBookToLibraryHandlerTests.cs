using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Lending.Cqrs;
using Lending.Domain.AcceptConnection;
using Lending.Domain.AddBookToCollection;
using Lending.Domain.Model;
using Lending.Domain.Persistence;
using Mono.Security.Authenticode;
using NUnit.Framework;
using ServiceStack.Text;

namespace Tests.Domain
{
    public class AddBookToLibraryHandlerTests : FixtureWithEventStoreAndNHibernate
    {
        /// <summary>
        /// GIVEN User1 is a Registered User AND Book1 is not Published
        /// WHEN User1 adds Book1 to her Collection
        /// THEN Book1 is Published in the system AND Book1 is added to User1's Collection
        /// </summary>
        [Test]
        public void AddingUnpublishedBookToCollectionShouldPublishBookAndAddToCollection()
        {
            var processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            SaveAggregates(user1);

            var registeredUser1 = new RegisteredUser(user1.Id, user1.UserName);
            SaveEntities(registeredUser1);

            var command = new AddBookToCollection(processId, user1.Id, user1.Id, "Book1", "Author1", "ISBNNumber");
            var expectedResult = new Result();
            var bookAddedToLibrary = new BookAddedToLibrary(processId, user1.Id, command.Title, command.Author, command.Isbn);

            var sut = new AddBookToCollectionHandler(() => Session, () => Repository);
            Result actualResult = sut.HandleCommand(command);
            CommitTransactionAndOpenNew();
            WriteRepository();

            actualResult.ShouldEqual(expectedResult);

            StreamEventsSlice  slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(2));
            var value = Encoding.UTF8.GetString(slice.Events[1].Event.Data);
            BookAddedToLibrary actualAddedToLibrary = value.FromJson<BookAddedToLibrary>();
            actualAddedToLibrary.ShouldEqual(bookAddedToLibrary);

        }

        /// <summary>
        /// GIVEN User1 is a Registered User AND Book1 is not Published
        /// WHEN User1 adds Book1 to her Collection
        /// THEN Book1 is Published in the system AND Book1 is added to User1's Collection
        /// </summary>
        [Test]
        public void AddingPublishedBookToCollectionShouldAddToCollection()
        {
            var processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            SaveAggregates(user1);

            var registeredUser1 = new RegisteredUser(user1.Id, user1.UserName);
            SaveEntities(registeredUser1);
            CommitTransactionAndOpenNew();

            var command = new AddBookToCollection(processId, user1.Id, user1.Id, "Title", "Author", "Isbn");
            var expectedResult = new Result();
            var expectedBookAddedToCollection = new BookAddedToLibrary(processId, user1.Id, command.Title, command.Author, command.Isbn);

            var sut = new AddBookToCollectionHandler(() => Session, () => Repository);
            Result actualResult = sut.HandleCommand(command);
            CommitTransactionAndOpenNew();
            WriteRepository();

            actualResult.ShouldEqual(expectedResult);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(2));
            var value = Encoding.UTF8.GetString(slice.Events[1].Event.Data);
            BookAddedToLibrary actualAddedToLibrary = value.FromJson<BookAddedToLibrary>();
            actualAddedToLibrary.ShouldEqual(expectedBookAddedToCollection);

        }

        /// <summary>
        /// GIVEN User1 is a registered user AND Book1 is Published AND Book1 is in User1's Collection
        /// WHEN User1 adds Book1 to her Collection
        /// THEN User1 is notified that Book1 is already in her Collection
        /// </summary>
        [Test]
        public void AddingPublishedBookAlreadyInCollectionShouldFail()
        {
            var processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            user1.AddBookToLibrary(processId, "Title", "Authority", "Isbn");
            SaveAggregates(user1);

            var registeredUser1 = new RegisteredUser(user1.Id, user1.UserName);
            SaveEntities(registeredUser1);
            CommitTransactionAndOpenNew();

            var command = new AddBookToCollection(processId, user1.Id, user1.Id, "Title", "Authority", "Isbn");
            var expectedResult = new Result(User.BookAlreadyInLibrary);

            var sut = new AddBookToCollectionHandler(() => Session, () => Repository);
            Result actualResult = sut.HandleCommand(command);
            CommitTransactionAndOpenNew();
            WriteRepository();

            actualResult.ShouldEqual(expectedResult);

            StreamEventsSlice  slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(2));

        }

    }
}
