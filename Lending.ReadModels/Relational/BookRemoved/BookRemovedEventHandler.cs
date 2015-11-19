using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Domain.RegisterUser;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.ReadModels.Relational.BookAdded;
using NHibernate;

namespace Lending.ReadModels.Relational.BookRemoved
{
    public class BookRemovedEventHandler : Lending.Cqrs.EventHandler<BookRemovedFromLibrary>
    {
        private readonly Func<ISession> getSession;

        public BookRemovedEventHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        public override void When(BookRemovedFromLibrary @event)
        {
            ISession session = getSession();

            LibraryBook bookToRemove = session.QueryOver<LibraryBook>()
                .Where(x => x.OwnerId == @event.AggregateId)
                .Where(x => x.Title == @event.Title)
                .Where(x => x.Author == @event.Author)
                .Where(x => x.Isbn == @event.Isbn)
                .SingleOrDefault();

            session.Delete(bookToRemove);
        }
    }
}
