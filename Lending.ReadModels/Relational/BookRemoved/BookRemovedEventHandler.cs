using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            LibraryBook bookToRemove = Session.QueryOver<LibraryBook>()
                .Where(x => x.Title == @event.Title)
                .Where(x => x.Author == @event.Author)
                .Where(x => x.Isbn == @event.Isbn)
                .Where(x => x.LibraryId == @event.AggregateId)
                .SingleOrDefault();

            Session.Delete(bookToRemove);
        }
    }
}
