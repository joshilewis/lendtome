using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
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
            IEnumerable<LibraryLink> connectedLibraries = Session.QueryOver<LibraryLink>()
                .Where(x => x.RequestingAdministratorId == message.UserId || 
                x.AcceptingAdministratorId == message.UserId)
                .List();
            if (connectedLibraries.IsEmpty()) return new BookSearchResult[] {};

            List<Guid> connectedUserIds = new List<Guid>();
            connectedUserIds.AddRange(connectedLibraries.Select(x => x.AcceptingLibraryId));
            connectedUserIds.AddRange(connectedLibraries.Select(x => x.RequestingLibraryId));
            connectedUserIds = connectedUserIds.Distinct().ToList();

            BookSearchResult[] payload = Session.QueryOver<LibraryBook>()
                .Where(Restrictions.On<LibraryBook>(x => x.Title).IsInsensitiveLike("%" + message.SearchString + "%") ||
                       Restrictions.On<LibraryBook>(x => x.Author).IsInsensitiveLike("%" + message.SearchString + "%"))
                .WhereRestrictionOn(x => x.LibraryId).IsIn(connectedUserIds)
                .List()
                .Select(x =>
                    new BookSearchResult(x.LibraryId, x.LibraryName, x.AdministratorPicture, x.Title, x.Author, x.Isbn,
                        x.PublishYear, x.CoverPicture))
                .ToArray();

            return payload;
        }

    }
}