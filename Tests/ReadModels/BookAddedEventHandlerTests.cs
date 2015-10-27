using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.RegisterUser;
using Lending.ReadModels.Relational.BookAdded;
using NUnit.Framework;

namespace Tests.ReadModels
{
    [TestFixture]
    public class BookAddedEventHandlerTests : FixtureWithEventStoreAndNHibernate
    {
        [Test]
        public void Test()
        {
            RegisteredUser user = new RegisteredUser(1,Guid.NewGuid(), "user name");
            SaveEntities(user);
            CommitTransactionAndOpenNew();

            var bookAdded = new BookAddedToLibrary(Guid.NewGuid(), user.Id, "Title", "Author", "Isbn");
            var expectedLibraryBook = new LibraryBook(bookAdded.ProcessId, user.Id, "Title", "Author", "Isbn");

            var sut = new BookAddedEventHandler(() => Session);
            sut.When(bookAdded);

            CommitTransactionAndOpenNew();

            var booksAdded = Session.QueryOver<LibraryBook>()
                .List();

            Assert.That(booksAdded.Count, Is.EqualTo(1));
            booksAdded[0].ShouldEqual(expectedLibraryBook);


        }
    }
}
