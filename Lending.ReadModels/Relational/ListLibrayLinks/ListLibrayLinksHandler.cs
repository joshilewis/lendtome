using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs.Query;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.LinkAccepted;
using Lending.ReadModels.Relational.LinkRequested;
using NHibernate;

namespace Lending.ReadModels.Relational.ListLibrayLinks
{
    public class ListLibrayLinksHandler : NHibernateQueryHandler<ListLibraryLinks>,
        IAuthenticatedQueryHandler<ListLibraryLinks>
    {
        public ListLibrayLinksHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        {
        }

        public override object Handle(ListLibraryLinks query)
        {
            LibraryLink libraryLinkAlias = null;
            OpenedLibrary acceptingLibraryAlias = null;
            OpenedLibrary requestingLibraryAlias = null;

            return Session.QueryOver<LibraryLink>(() => libraryLinkAlias)
                .JoinAlias(x => x.AcceptingLibrary, () => acceptingLibraryAlias)
                .JoinAlias(x => x.RequestingLibrary, () => requestingLibraryAlias)
                .Where(
                    () =>
                        acceptingLibraryAlias.AdministratorId == query.UserId ||
                        requestingLibraryAlias.AdministratorId == query.UserId)
                .List()
                .Select(x =>
                {
                    if (x.AcceptingLibrary.Id == query.UserId)
                        return new LibrarySearchResult(x.RequestingLibrary.Id, x.RequestingLibrary.Name);
                    return new LibrarySearchResult(x.AcceptingLibrary.Id, x.AcceptingLibrary.Name);
                })
                .ToArray();
        }
    }
}
