using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Lending.Cqrs;
using Lending.Domain.Model;
using NHibernate;
using NUnit.Framework;
using ServiceStack.Text;

namespace Tests.Domain
{
    /// <summary>
    /// https://github.com/joshilewis/lending/issues/11
    /// As a User I want to Remove Books from my Collection so that my Connections can see that I no longer own the book.
    /// </summary>
    [TestFixture]
    public class RemoveBookFromLibraryHandlerTests : FixtureWithEventStoreAndNHibernate
    {
        /// <summary>
        /// GIVEN 'User1' is a Registered user AND Book1 is not in User1's Library
        /// WHEN User1 Removes Book1 from her Library
        /// THEN User1 is notified that Book1 is not in her Library        
        /// </summary>
        [Test]
        public void RemoveBookFromLibraryShouldSucceed()
        {
            var processId = Guid.NewGuid();
            var user1 = User.Register(processId, Guid.NewGuid(), "User 1", "email1");
            user1.AddBookToLibrary(processId, "Title", "Author", "Isbn");
            SaveAggregates(user1);

            var command = new RemoveBookFromCollection(processId, user1.Id, user1.Id, "Title", "Author", "Isbn");
            var expectedResult = new Result();
            var expectedEvent = new BookRemovedFromLibrary(processId, user1.Id, "Title", "Author", "Isbn");

            var sut = new RemoveBookFromLibraryHandler(() => Session, () => Repository);
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
            Assert.Fail();
        }

    }

    public class RemoveBookFromLibraryHandler : AuthenticatedCommandHandler<RemoveBookFromCollection, Result>
    {
        public RemoveBookFromLibraryHandler(Func<ISession> getSession, Func<IRepository> getRepository)
            : base(getSession, getRepository)
        {
        }

        public override Result HandleCommand(RemoveBookFromCollection command)
        {
            throw new NotImplementedException();
        }
    }

    public class RemoveBookFromCollection : AuthenticatedCommand
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }

        public RemoveBookFromCollection(Guid processId, Guid aggregateId, Guid userId, string title, string author,
            string isbn) : base(processId, aggregateId, userId)
        {
            Title = title;
            Author = author;
            Isbn = isbn;
        }
    }

    public class BookRemovedFromLibrary : Event
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }

        public BookRemovedFromLibrary(Guid processId, Guid aggregateId, string title, string author, string isbn)
            : base(processId, aggregateId)
        {
            Title = title;
            Author = author;
            Isbn = isbn;
        }
    }
}
