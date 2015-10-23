using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Lending.Cqrs;
using Lending.Domain.Model;
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
        /// <summary>
        /// GIVEN 'User1' is a Registered user AND Book1 is not in User1's Library
        /// WHEN User1 Removes Book1 from her Library
        /// THEN User1 is notified that Book1 is not in her Library        
        /// </summary>
        [Test]
        public void RemoveBookInLibraryShouldSucceed()
        {
            var processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            user1.AddBookToLibrary(processId, "Title", "Author", "Isbn");
            SaveAggregates(user1);

            var command = new RemoveBookFromLibrary(processId, user1.Id, user1.Id, "Title", "Author", "Isbn");
            var expectedResult = new Result();
            var expectedEvent = new BookRemovedFromLibrary(processId, user1.Id, "Title", "Author", "Isbn");

            var sut = new RemoveBookFromLibraryHandler(() => Repository, () => EventRepository);
            Result actualResult = sut.HandleCommand(command);
            CommitTransactionAndOpenNew();
            WriteRepository();

            actualResult.ShouldEqual(expectedResult);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(3));
            var value = Encoding.UTF8.GetString(slice.Events[1].Event.Data);
            BookRemovedFromLibrary actualEvent = value.FromJson<BookRemovedFromLibrary>();
            actualEvent.ShouldEqual(expectedEvent);

        }

        /// <summary>
        /// GIVEN 'User1' is a Registered User AND Book 'Book1' is in User1's Library 
        /// WHEN User1 Removes Book1 from her Library
        /// THEN Book1 is Removed from User1's Library
        /// </summary>
        [Test]
        public void RemoveBookNotInLibraryShouldFail()
        {
            var processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            SaveAggregates(user1);

            var command = new RemoveBookFromLibrary(processId, user1.Id, user1.Id, "Title", "Author", "Isbn");
            var expectedResult = new Result(User.BookNotInLibrary);

            var sut = new RemoveBookFromLibraryHandler(() => Repository, () => EventRepository);
            Result actualResult = sut.HandleCommand(command);
            CommitTransactionAndOpenNew();
            WriteRepository();

            actualResult.ShouldEqual(expectedResult);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(1));

        }

    }
}
