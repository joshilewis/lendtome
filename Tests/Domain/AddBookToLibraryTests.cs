using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Lending.Cqrs;
using Lending.Domain.AcceptConnection;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.Model;
using Lending.Domain.Persistence;
using Mono.Security.Authenticode;
using NUnit.Framework;
using ServiceStack.Text;

namespace Tests.Domain
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
            var processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            SaveAggregates(user1);

            var command = new AddBookToLibrary(processId, user1.Id, user1.Id, "Title", "Author", "Isbn");
            var expectedResult = new Result();
            var expectedBookAddedToCollection = new BookAddedToLibrary(processId, user1.Id, command.Title, command.Author, command.Isbn);

            var sut = new AddBookToLibraryHandler(() => Session, () => EventRepository);
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
        /// GIVEN User1 is a Registered User AND Book1 is in User1's Library
        /// WHEN User1 Adds Book1 to her Library
        /// THEN User1 is notified that Book1 is already in her Library
        /// </summary>
        [Test]
        public void AddingDuplicateBookToLibraryShouldFail()
        {
            var processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            user1.AddBookToLibrary(processId, "Title", "Authority", "Isbn");
            SaveAggregates(user1);

            var command = new AddBookToLibrary(processId, user1.Id, user1.Id, "Title", "Authority", "Isbn");
            var expectedResult = new Result(User.BookAlreadyInLibrary);

            var sut = new AddBookToLibraryHandler(() => Session, () => EventRepository);
            Result actualResult = sut.HandleCommand(command);
            CommitTransactionAndOpenNew();
            WriteRepository();

            actualResult.ShouldEqual(expectedResult);

            StreamEventsSlice  slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(2));

        }

        /// <summary>
        /// GIVEN User1 is a Registered User AND User1 has Added and Removed Book1 from her Library
        /// WHEN User1 Adds Book1 to her Library
        /// THEN Book1 is Added to User1's Library
        /// </summary>
        [Test]
        public void AddingPreviouslyRemovedBookToLibraryShouldSucceed()
        {
            var processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            user1.AddBookToLibrary(processId, "Title", "Author", "Isbn");
            user1.RemoveBookFromLibrary(processId, "Title", "Author", "Isbn");
            SaveAggregates(user1);

            var command = new AddBookToLibrary(processId, user1.Id, user1.Id, "Title", "Author", "Isbn");
            var expectedResult = new Result();
            var expectedBookAddedToCollection = new BookAddedToLibrary(processId, user1.Id, command.Title, command.Author, command.Isbn);

            var sut = new AddBookToLibraryHandler(() => Session, () => EventRepository);
            Result actualResult = sut.HandleCommand(command);
            CommitTransactionAndOpenNew();
            WriteRepository();

            actualResult.ShouldEqual(expectedResult);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(4));
            var value = Encoding.UTF8.GetString(slice.Events[1].Event.Data);
            BookAddedToLibrary actualAddedToLibrary = value.FromJson<BookAddedToLibrary>();
            actualAddedToLibrary.ShouldEqual(expectedBookAddedToCollection);

        }

    }
}
