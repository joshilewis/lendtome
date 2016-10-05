using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Command;
using Joshilewis.Cqrs.Query;
using Joshilewis.Infrastructure.EventRouting;
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
        public SearchForLibraryModule(NHibernateUnitOfWork unitOfWork, IMessageHandler<SearchForLibrary> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/{searchstring}")
        {
        }
    }

    public class RequestLinkModule: PostModule<RequestLink>
    {
        public RequestLinkModule(EventStoreUnitOfWork unitOfWork, ICommandHandler<RequestLink> commandHandler,
            NHibernateUnitOfWork relationalUnitOfWork, EventDispatcher eventDispatcher)
            : base(unitOfWork, commandHandler, "/libraries/{AggregateId}/links/request/", relationalUnitOfWork,
                eventDispatcher)
        {
        }

    }

    public class AcceptLinkModule : PostModule<AcceptLink>
    {
        public AcceptLinkModule(EventStoreUnitOfWork unitOfWork, ICommandHandler<AcceptLink> commandHandler,
            NHibernateUnitOfWork relationalUnitOfWork, EventDispatcher eventDispatcher)
            : base(unitOfWork, commandHandler, "/libraries/{AggregateId}/links/accept/", relationalUnitOfWork,
                eventDispatcher)
        {
        }

    }

    public class AddBookModule : PostModule<AddBookToLibrary>
    {
        public AddBookModule(EventStoreUnitOfWork unitOfWork, ICommandHandler<AddBookToLibrary> commandHandler,
            NHibernateUnitOfWork relationalUnitOfWork, EventDispatcher eventDispatcher)
            : base(unitOfWork, commandHandler, "/libraries/{AggregateId}/books/add", relationalUnitOfWork,
                eventDispatcher)
        {
        }

    }

    public class RemoveBookModule : PostModule<RemoveBookFromLibrary>
    {
        public RemoveBookModule(EventStoreUnitOfWork unitOfWork, ICommandHandler<RemoveBookFromLibrary> commandHandler,
            NHibernateUnitOfWork relationalUnitOfWork, EventDispatcher eventDispatcher)
            : base(unitOfWork, commandHandler, "/libraries/{AggregateId}/books/remove", relationalUnitOfWork,
                eventDispatcher)
        {
        }

    }
    public class OpenLibraryModule : PostModule<OpenLibrary>
    {
        public OpenLibraryModule(EventStoreUnitOfWork unitOfWork, ICommandHandler<OpenLibrary> commandHandler,
            NHibernateUnitOfWork relationalUnitOfWork, EventDispatcher eventDispatcher)
            : base(unitOfWork, commandHandler, "/libraries/", relationalUnitOfWork,
                eventDispatcher)
        {
        }

    }

    public class ListLibrariesModule : AuthenticatedGetModule<ListLibraries>
    {
        public ListLibrariesModule(NHibernateUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListLibraries> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/")
        {
        }
    }

    public class SearchForBookModule : AuthenticatedGetModule<SearchForBook>
    {
        public SearchForBookModule(NHibernateUnitOfWork unitOfWork, IAuthenticatedQueryHandler<SearchForBook> queryHandler)
            : base(unitOfWork, queryHandler, "/books/{SearchString}")
        {
        }
    }

    public class ListRequestedLinksModule : AuthenticatedGetModule<ListRequestedLinks>
    {
        public ListRequestedLinksModule(NHibernateUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListRequestedLinks> queryHandler)
            : base(unitOfWork, queryHandler, "/libraries/{AggregateId}/links/sent")
        {
        }
    }

    public class ListReceivedLinksModule : AuthenticatedGetModule<ListReceivedLinks>
    {
        public ListReceivedLinksModule(NHibernateUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListReceivedLinks> queryHandler)
            : base(unitOfWork, queryHandler, "/libraries/{AggregateId}/links/received")
        {
        }
    }

    public class ListLibraryLinksModule : AuthenticatedGetModule<ListLibraryLinks>
    {
        public ListLibraryLinksModule(NHibernateUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListLibraryLinks> queryHandler)
            : base(unitOfWork, queryHandler, "/libraries/{AggregateId}/links/")
        {
        }
    }

    public class ListLibraryBooksModule : AuthenticatedGetModule<ListLibraryBooks>
    {
        public ListLibraryBooksModule(NHibernateUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListLibraryBooks> queryHandler)
            : base(unitOfWork, queryHandler, "/libraries/{AggregateId}/books/")
        {
        }
    }


}
