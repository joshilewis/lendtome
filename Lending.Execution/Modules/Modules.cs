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
    public class SearchForLibraryModule : GetModule<SearchForLibrary>
    {
        public SearchForLibraryModule(IUnitOfWork unitOfWork, IMessageHandler<SearchForLibrary> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/{searchstring}")
        {
        }
    }

    public class RequestLinkModule: PostModule<RequestLink>
    {
        public RequestLinkModule(IUnitOfWork unitOfWork, ICommandHandler<RequestLink> commandHandler)
            : base(unitOfWork, commandHandler, "/libraries/{AggregateId}/links/request/")
        {
        }

    }

    public class AcceptLinkModule : PostModule<AcceptLink>
    {
        public AcceptLinkModule(IUnitOfWork unitOfWork, ICommandHandler<AcceptLink> commandHandler)
            : base(unitOfWork, commandHandler, "/libraries/{AggregateId}/links/accept/")
        {
        }

    }

    public class AddBookModule : PostModule<AddBookToLibrary>
    {
        public AddBookModule(IUnitOfWork unitOfWork, ICommandHandler<AddBookToLibrary> commandHandler)
            : base(unitOfWork, commandHandler, "/libraries/{AggregateId}/books/add")
        {
        }

    }

    public class RemoveBookModule : PostModule<RemoveBookFromLibrary>
    {
        public RemoveBookModule(IUnitOfWork unitOfWork, ICommandHandler<RemoveBookFromLibrary> commandHandler)
            : base(unitOfWork, commandHandler, "/libraries/{AggregateId}/books/remove")
        {
        }

    }
    public class OpenLibraryModule : PostModule<OpenLibrary>
    {
        public OpenLibraryModule(IUnitOfWork unitOfWork, ICommandHandler<OpenLibrary> commandHandler)
            : base(unitOfWork, commandHandler, "/libraries/")
        {
        }

    }

    public class ListLibrariesModule : AuthenticatedGetModule<ListLibraries>
    {
        public ListLibrariesModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListLibraries> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/")
        {
        }
    }

    public class SearchForBookModule : AuthenticatedGetModule<SearchForBook>
    {
        public SearchForBookModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<SearchForBook> queryHandler)
            : base(unitOfWork, queryHandler, "/books/{SearchString}")
        {
        }
    }

    public class ListRequestedLinksModule : AuthenticatedGetModule<ListRequestedLinks>
    {
        public ListRequestedLinksModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListRequestedLinks> queryHandler)
            : base(unitOfWork, queryHandler, "/libraries/{AggregateId}/links/sent")
        {
        }
    }

    public class ListReceivedLinksModule : AuthenticatedGetModule<ListReceivedLinks>
    {
        public ListReceivedLinksModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListReceivedLinks> queryHandler)
            : base(unitOfWork, queryHandler, "/libraries/{AggregateId}/links/received")
        {
        }
    }

    public class ListLibraryLinksModule : AuthenticatedGetModule<ListLibraryLinks>
    {
        public ListLibraryLinksModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListLibraryLinks> queryHandler)
            : base(unitOfWork, queryHandler, "/libraries/{AggregateId}/links/")
        {
        }
    }

    public class ListLibraryBooksModule : AuthenticatedGetModule<ListLibraryBooks>
    {
        public ListLibraryBooksModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListLibraryBooks> queryHandler)
            : base(unitOfWork, queryHandler, "/libraries/{AggregateId}/books/")
        {
        }
    }


}
