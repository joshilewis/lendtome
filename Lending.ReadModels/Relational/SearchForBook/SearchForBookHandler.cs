using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lending.Cqrs;
using Lending.Cqrs.Query;
using Lending.ReadModels.Relational.BookAdded;
using Lending.ReadModels.Relational.ConnectionAccepted;
using NHibernate;
using NHibernate.Criterion;

namespace Lending.ReadModels.Relational.SearchForBook
{
    public class SearchForBookHandler : MessageHandler<SearchForBook, Result>, IQueryHandler<SearchForBook, Result>
    {
        public const string UserHasNoConnection = "User has no connections";

        private readonly Func<ISession> getSession;

        public SearchForBookHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        public override Result Handle(SearchForBook message)
        {
            ISession session = getSession();

            int numberOfConnections = session.QueryOver<UserConnection>()
                .Where(x => x.AcceptingUserId == message.UserId || x.RequestingUserId == message.UserId)
                .RowCount();

            if (numberOfConnections == 0)
                return new Result<BookSearchResult[]>(UserHasNoConnection, new BookSearchResult[] {});

            IEnumerable<UserConnection> connectedUsers = session.QueryOver<UserConnection>()
                .Where(x => x.RequestingUserId == message.UserId || x.AcceptingUserId == message.UserId)
                .List();

            List<Guid> connectedUserIds = new List<Guid>();
            connectedUserIds.AddRange(connectedUsers.Select(x => x.AcceptingUserId));
            connectedUserIds.AddRange(connectedUsers.Select(x => x.RequestingUserId));
            connectedUserIds = connectedUserIds.Distinct().ToList();

            BookSearchResult[] payload = session.QueryOver<LibraryBook>()
                .Where(Restrictions.On<LibraryBook>(x => x.Title).IsInsensitiveLike("%" + message.SearchString + "%") ||
                    Restrictions.On<LibraryBook>(x => x.Author).IsInsensitiveLike("%" + message.SearchString + "%"))
                    .WhereRestrictionOn(x => x.OwnerId).IsIn(connectedUserIds)
                .List()
                .Select(x => new BookSearchResult(x.OwnerId, x.OwnerName, x.Title, x.Author))
                .ToArray();

            return new Result<BookSearchResult[]>(payload);
        }

    }
}