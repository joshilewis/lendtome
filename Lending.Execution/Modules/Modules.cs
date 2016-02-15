using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Command;
using Joshilewis.Cqrs.Query;
using Joshilewis.Infrastructure.Nancy;
using Joshilewis.Infrastructure.UnitOfWork;
using Lending.Domain.AcceptLink;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.OpenLibrary;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.Domain.RequestLink;
using Lending.ReadModels.Relational.LinkRequested;
using Lending.ReadModels.Relational.ListLibraries;
using Lending.ReadModels.Relational.ListLibraryBooks;
using Lending.ReadModels.Relational.ListLibrayLinks;
using Lending.ReadModels.Relational.ListReceivedLinks;
using Lending.ReadModels.Relational.ListRequestedLinks;
using Lending.ReadModels.Relational.SearchForBook;
using Lending.ReadModels.Relational.SearchForLibrary;
using Nancy;

namespace Lending.Execution.Modules
{
    public class SearchForLibraryModule : GetModule<SearchForLibrary, LibrarySearchResult[]>
    {
        public SearchForLibraryModule(IUnitOfWork unitOfWork, IMessageHandler<SearchForLibrary, LibrarySearchResult[]> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/{searchstring}")
        {
        }
    }

    public class RequestLinkModule: PostModule<RequestLink, Result>
    {
        public RequestLinkModule(IUnitOfWork unitOfWork, ICommandHandler<RequestLink> commandHandler)
            : base(unitOfWork, commandHandler, "/libraries/{AggregateId}/links/request/")
        {
        }

    }

    public class AcceptLinkModule : PostModule<AcceptLink, Result>
    {
        public AcceptLinkModule(IUnitOfWork unitOfWork, ICommandHandler<AcceptLink> commandHandler)
            : base(unitOfWork, commandHandler, "/libraries/{AggregateId}/links/accept/")
        {
        }

    }

    public class AddBookModule : PostModule<AddBookToLibrary, Result>
    {
        public AddBookModule(IUnitOfWork unitOfWork, ICommandHandler<AddBookToLibrary> commandHandler)
            : base(unitOfWork, commandHandler, "/libraries/{AggregateId}/books/add")
        {
        }

    }

    public class RemoveBookModule : PostModule<RemoveBookFromLibrary, Result>
    {
        public RemoveBookModule(IUnitOfWork unitOfWork, ICommandHandler<RemoveBookFromLibrary> commandHandler)
            : base(unitOfWork, commandHandler, "/libraries/{AggregateId}/books/remove")
        {
        }

    }
    public class OpenLibraryModule : PostModule<OpenLibrary, Result>
    {
        public OpenLibraryModule(IUnitOfWork unitOfWork, ICommandHandler<OpenLibrary> commandHandler)
            : base(unitOfWork, commandHandler, "/libraries/")
        {
        }

    }

    public class ListLibrariesModule : AuthenticatedGetModule<ListLibraries, LibrarySearchResult[]>
    {
        public ListLibrariesModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListLibraries, LibrarySearchResult[]> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/")
        {
        }
    }

    public class SearchForBookModule : AuthenticatedGetModule<SearchForBook, BookSearchResult[]>
    {
        public SearchForBookModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<SearchForBook, BookSearchResult[]> queryHandler)
            : base(unitOfWork, queryHandler, "/books/{SearchString}")
        {
        }
    }

    public class ListRequestedLinksModule : AuthenticatedGetModule<ListRequestedLinks, RequestedLink[]>
    {
        public ListRequestedLinksModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListRequestedLinks, RequestedLink[]> queryHandler)
            : base(unitOfWork, queryHandler, "/libraries/{AggregateId}/links/sent")
        {
        }
    }

    public class ListReceivedLinksModule : AuthenticatedGetModule<ListReceivedLinks, RequestedLink[]>
    {
        public ListReceivedLinksModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListReceivedLinks, RequestedLink[]> queryHandler)
            : base(unitOfWork, queryHandler, "/libraries/{AggregateId}/links/received")
        {
        }
    }

    public class ListLibraryLinksModule : AuthenticatedGetModule<ListLibraryLinks, LibrarySearchResult[]>
    {
        public ListLibraryLinksModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListLibraryLinks, LibrarySearchResult[]> queryHandler)
            : base(unitOfWork, queryHandler, "/libraries/{AggregateId}/links/")
        {
        }
    }

    public class ListLibraryBooksModule : AuthenticatedGetModule<ListLibraryBooks, BookSearchResult[]>
    {
        public ListLibraryBooksModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListLibraryBooks, BookSearchResult[]> queryHandler)
            : base(unitOfWork, queryHandler, "/libraries/{AggregateId}/books/")
        {
        }
    }


}
