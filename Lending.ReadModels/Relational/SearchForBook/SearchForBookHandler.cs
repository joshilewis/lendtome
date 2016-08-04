using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Joshilewis.Cqrs.Query;
using Lending.ReadModels.Relational.BookAdded;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.LinkAccepted;
using NHibernate;
using NHibernate.Criterion;

namespace Lending.ReadModels.Relational.SearchForBook
{
    public class SearchForBookHandler : NHibernateQueryHandler<SearchForBook>, IAuthenticatedQueryHandler<SearchForBook>
    {
        public const string UserHasNoConnection = "User has no connections";

        public SearchForBookHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override object Handle(SearchForBook message)
        {
            LibraryLink libraryLinkAlias = null;
            OpenedLibrary acceptingLibraryAlias = null;
            OpenedLibrary requestingLibraryAlias = null;

            int numberOfConnections = Session.QueryOver<LibraryLink>(() => libraryLinkAlias)
                .JoinAlias(x => x.AcceptingLibrary, () => acceptingLibraryAlias)
                .JoinAlias(x => x.RequestingLibrary, () => requestingLibraryAlias)
                .Where(() => acceptingLibraryAlias.AdministratorId == message.UserId || requestingLibraryAlias.AdministratorId == message.UserId)
                .RowCount();

            if (numberOfConnections == 0) return new BookSearchResult[] {};

            IEnumerable<LibraryLink> connectedUsers = Session.QueryOver<LibraryLink>(() => libraryLinkAlias)
                .JoinAlias(x => x.AcceptingLibrary, () => acceptingLibraryAlias)
                .JoinAlias(x => x.RequestingLibrary, () => requestingLibraryAlias)
                .Where(() => requestingLibraryAlias.AdministratorId == message.UserId || acceptingLibraryAlias.AdministratorId == message.UserId)
                .List();

            List<Guid> connectedUserIds = new List<Guid>();
            connectedUserIds.AddRange(connectedUsers.Select(x => x.AcceptingLibrary.Id));
            connectedUserIds.AddRange(connectedUsers.Select(x => x.RequestingLibrary.Id));
            connectedUserIds = connectedUserIds.Distinct().ToList();

            BookSearchResult[] payload = Session.QueryOver<LibraryBook>()
                .Where(Restrictions.On<LibraryBook>(x => x.Title).IsInsensitiveLike("%" + message.SearchString + "%") ||
                    Restrictions.On<LibraryBook>(x => x.Author).IsInsensitiveLike("%" + message.SearchString + "%"))
                    .JoinQueryOver(x => x.Library)
                    .WhereRestrictionOn(x => x.Id).IsIn(connectedUserIds)
                .List()
                .Select(x => new BookSearchResult(x.Library.Id, x.LibraryName, x.Title, x.Author, x.Isbn, x.PublishYear))
                .ToArray();

            return payload;
        }

    }
}