using Lending.Cqrs;
using Lending.Cqrs.Query;
using Lending.Domain.AcceptLink;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.OpenLibrary;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.Domain.RequestLink;
using Lending.Execution.UnitOfWork;
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

    public class RequestLinkModule: PutModule<RequestLink, Result>
    {
        public RequestLinkModule(IUnitOfWork unitOfWork, IMessageHandler<RequestLink, Result> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/{AggregateId}/links/request/")
        {
        }

    }

    public class AcceptLinkModule : PutModule<AcceptLink, Result>
    {
        public AcceptLinkModule(IUnitOfWork unitOfWork, IMessageHandler<AcceptLink, Result> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/{AggregateId}/links/accept/")
        {
        }

    }

    public class AddBookModule : PutModule<AddBookToLibrary, Result>
    {
        public AddBookModule(IUnitOfWork unitOfWork, IMessageHandler<AddBookToLibrary, Result> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/{AggregateId}/books/add")
        {
        }

    }

    public class RemoveBookModule : PutModule<RemoveBookFromLibrary, Result>
    {
        public RemoveBookModule(IUnitOfWork unitOfWork, IMessageHandler<RemoveBookFromLibrary, Result> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/{AggregateId}/books/remove")
        {
        }

    }
    public class OpenLibraryModule : PutModule<OpenLibrary, Result>
    {
        public OpenLibraryModule(IUnitOfWork unitOfWork, IMessageHandler<OpenLibrary, Result> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/")
        {
        }

    }

    public class ListLibrariesModule : GetModule<SearchForLibrary, Result>
    {
        public ListLibrariesModule(IUnitOfWork unitOfWork, IMessageHandler<SearchForLibrary, Result> messageHandler)
            : base(unitOfWork, messageHandler, "/libraries/")
        {
        }
    }

    public class SearchBookModule : GetModule<SearchForBook, Result>
    {
        public SearchBookModule(IUnitOfWork unitOfWork, IMessageHandler<SearchForBook, Result> messageHandler)
            : base(unitOfWork, messageHandler, "/books/{SearchString}")
        {
        }
    }


}
