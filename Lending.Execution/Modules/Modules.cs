using System;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.AcceptLink;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.Model;
using Lending.Domain.RequestLink;
using Lending.Execution.UnitOfWork;
using Lending.ReadModels.Relational.SearchForLibrary;

namespace Lending.Execution.Modules
{
    public class SearchForUserModule : GetModule<SearchForLibrary, Result>
    {
        public SearchForUserModule(IUnitOfWork unitOfWork, IMessageHandler<SearchForLibrary, Result> messageHandler)
            : base(unitOfWork, messageHandler)
        {
        }

        protected override string Path => "/libraries/{searchstring}";
    }

    public class RequestLinkModule: PostModule<RequestLink, Result, Library>
    {
        public RequestLinkModule(IUnitOfWork unitOfWork, ICommandHandler<RequestLink, Result, Library> messageHandler, Func<IEventRepository> eventRepositoryFunc)
            : base(unitOfWork, messageHandler, eventRepositoryFunc)
        {
        }

        protected override string Path => "/links/request/";
    }

    public class AcceptLinkModule : PostModule<AcceptLink, Result, Library>
    {
        public AcceptLinkModule(IUnitOfWork unitOfWork, ICommandHandler<AcceptLink, Result, Library> messageHandler, Func<IEventRepository> eventRepositoryFunc)
            : base(unitOfWork, messageHandler, eventRepositoryFunc)
        {
        }

        protected override string Path => "/links/accept/";
    }

    public class AddBookModule : PostModule<AddBookToLibrary, Result, Library>
    {
        public AddBookModule(IUnitOfWork unitOfWork, ICommandHandler<AddBookToLibrary, Result, Library> messageHandler, Func<IEventRepository> eventRepositoryFunc)
            : base(unitOfWork, messageHandler, eventRepositoryFunc)
        {
        }

        protected override string Path => "/books/add";
    }
}
