using Lending.Cqrs;
using Lending.Cqrs.Query;
using Lending.Domain.AcceptLink;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.Domain.RequestLink;
using Lending.Execution.UnitOfWork;
using Lending.ReadModels.Relational.SearchForLibrary;
using Nancy;

namespace Lending.Execution.Modules
{
    public class SearchForUserModule : GetModule<SearchForLibrary, Result>
    {
        public SearchForUserModule(IUnitOfWork unitOfWork, IMessageHandler<SearchForLibrary, Result> messageHandler)
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

    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get[""] = _ => "Hello World!";
        }
    }
}
