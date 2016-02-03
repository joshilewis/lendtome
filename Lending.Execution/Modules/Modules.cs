using Lending.Cqrs;
using Lending.Cqrs.Query;
using Lending.Domain.AcceptLink;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.OpenLibrary;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.Domain.RequestLink;
using Lending.Execution.UnitOfWork;
using Lending.ReadModels.Relational.ListLibraries;
using Lending.ReadModels.Relational.SearchForBook;
using Lending.ReadModels.Relational.SearchForLibrary;
using Nancy;

namespace Lending.Execution.Modules
{
    public class SearchForLibraryModule : GetModule<SearchForLibrary, Result>
    {
        public SearchForLibraryModule(IUnitOfWork unitOfWork, IMessageHandler<SearchForLibrary, Result> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/{searchstring}")
        {
        }
    }

    public class RequestLinkModule: PostModule<RequestLink, Result>
    {
        public RequestLinkModule(IUnitOfWork unitOfWork, IMessageHandler<RequestLink, Result> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/{AggregateId}/links/request/")
        {
        }

    }

    public class AcceptLinkModule : PostModule<AcceptLink, Result>
    {
        public AcceptLinkModule(IUnitOfWork unitOfWork, IMessageHandler<AcceptLink, Result> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/{AggregateId}/links/accept/")
        {
        }

    }

    public class AddBookModule : PostModule<AddBookToLibrary, Result>
    {
        public AddBookModule(IUnitOfWork unitOfWork, IMessageHandler<AddBookToLibrary, Result> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/{AggregateId}/books/add")
        {
        }

    }

    public class RemoveBookModule : PostModule<RemoveBookFromLibrary, Result>
    {
        public RemoveBookModule(IUnitOfWork unitOfWork, IMessageHandler<RemoveBookFromLibrary, Result> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/{AggregateId}/books/remove")
        {
        }

    }
    public class OpenLibraryModule : PostModule<OpenLibrary, Result>
    {
        public OpenLibraryModule(IUnitOfWork unitOfWork, IMessageHandler<OpenLibrary, Result> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/")
        {
        }

    }

    public class ListLibrariesModule : AuthenticatedGetModule<ListLibraries, Result>
    {
        public ListLibrariesModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<ListLibraries, Result> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/")
        {
        }
    }

    public class SearchForBookModule : AuthenticatedGetModule<SearchForBook, Result>
    {
        public SearchForBookModule(IUnitOfWork unitOfWork, IAuthenticatedQueryHandler<SearchForBook, Result> queryHandler)
            : base(unitOfWork, queryHandler, "/books/{SearchString}")
        {
        }
    }


}
