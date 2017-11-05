using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.ReadModels.Relational.BookAdded;
using NHibernate;

namespace Lending.ReadModels.Relational.BookRemoved
{
    public class BookRemovedEventHandler : NHibernateEventHandler<BookRemovedFromLibrary>
    {
        public BookRemovedEventHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override void When(BookRemovedFromLibrary @event)
        {
            Connection.Execute(
                "DELETE FROM \"LibraryBook\" WHERE \"Title\" = @title AND \"Author\" = @author AND \"Isbn\" = @isbn AND \"LibraryId\" = @libraryId",
                new {@event.Title, @event.Author, @event.Isbn, LibraryId = @event.AggregateId});
        }
    }
}
