using Lending.Cqrs;
using Lending.Cqrs.Query;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.RequestConnection;
using Lending.Execution.UnitOfWork;
using Lending.ReadModels.Relational.SearchForUser;

namespace Lending.Execution.Modules
{
    public class SearchForUserModule : GetModule<SearchForUser, Result>
    {
        public SearchForUserModule(IUnitOfWork unitOfWork, IMessageHandler<SearchForUser, Result> messageHandler)
            : base(unitOfWork, messageHandler)
        {
        }

        protected override string Path => "/users/{searchstring}";
    }

    public class RequestConnectionModule: PostModule<RequestConnection, Result>
    {
        public RequestConnectionModule(IUnitOfWork unitOfWork, IMessageHandler<RequestConnection, Result> messageHandler)
            : base(unitOfWork, messageHandler)
        {
        }

        protected override string Path => "/connections/request/{TargetUserId}/";
    }

    public class AddBookModule : PostModule<AddBookToLibrary, Result>
    {
        public AddBookModule(IUnitOfWork unitOfWork, IMessageHandler<AddBookToLibrary, Result> messageHandler)
            : base(unitOfWork, messageHandler)
        {
        }

        protected override string Path => "/books/add";
    }
}
