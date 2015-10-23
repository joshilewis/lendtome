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
using NUnit.Framework;
using ServiceStack.Text;

namespace Tests.Domain
{
    public class AddBookToCollectionHandlerTests : FixtureWithEventStoreAndNHibernate
    {
        /// <summary>
        /// GIVEN User 'User1' is a registered user AND Book 'Book1' does not exist in the system
        /// WHEN User1 adds Book1 to her Collection
        /// THEN Book1 is added to the system AND Book1 is added to User1's Collection
        /// </summary>
        [Test]
        public void AddingNewBookToCollectionShouldCreateBookAndAddToCollection()
        {
            var processId = Guid.NewGuid();
            var bookId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            SaveAggregates(user1);

            var registeredUser1 = new RegisteredUser(user1.Id, user1.UserName);
            SaveEntities(registeredUser1);

            var command = new AddBookToCollection(processId, user1.Id, user1.Id, "Book1", "Author1", "ISBNNumber");
            var expectedResult = new Result();
            var expectedBookAdded = new BookAdded(processId, bookId, command.Title, command.Author, command.Isbn);
            var expectedBookAddedToCollection = new BookAddedToCollection(processId, user1.Id, bookId);
            var expectedBookInDb = new AddedBook(bookId, command.Title, command.Author, command.Isbn);

            var sut = new AddBookToCollectionHandler(() => Session, () => Repository, () => bookId);
            Result actualResult = sut.HandleCommand(command);
            CommitTransactionAndOpenNew();
            WriteRepository();

            actualResult.ShouldEqual(expectedResult);

            AddedBook actualBookInDb = Session.Get<AddedBook>(bookId);
            actualBookInDb.ShouldEqual(expectedBookInDb);

            StreamEventsSlice slice = Connection.ReadStreamEventsForwardAsync($"book-{bookId}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(1));
            var value = Encoding.UTF8.GetString(slice.Events[0].Event.Data);
            BookAdded actualBookAdded = value.FromJson<BookAdded>();
            actualBookAdded.ShouldEqual(expectedBookAdded);

            slice = Connection.ReadStreamEventsForwardAsync($"user-{user1.Id}", 0, 10, false).Result;
            Assert.That(slice.Events.Length, Is.EqualTo(2));
            value = Encoding.UTF8.GetString(slice.Events[1].Event.Data);
            BookAddedToCollection actualAddedToCollection = value.FromJson<BookAddedToCollection>();
            actualAddedToCollection.ShouldEqual(expectedBookAddedToCollection);


        }

        /// <summary>
        /// GIVEN User 'User1' is a registered user AND Book 'Book1' already exists in the system
        /// WHEN User1 adds Book1 to her Collection
        /// THEN Book1 is added to User1's Collection
        /// </summary>
        [Test]
        public void AddingExistingBookToCollectionShouldAddToCollection()
        {

            Assert.Fail();
        }

        /// <summary>
        /// User 'User1' is a registered user AND Book 'Book1' exists in the system AND Book1 is in User1's Collection
        /// WHEN User1 adds Book1 to her Collection
        /// THEN Book1 is NOT added to User1's Collection AND the user is notified that Book1 is already in her Collection
        /// </summary>
        [Test]
        public void AddingBookAlreadyInCollectionShouldFail()
        {

            Assert.Fail();
        }

    }
}
