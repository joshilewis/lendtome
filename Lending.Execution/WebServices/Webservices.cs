using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain;
using Lending.Domain.AcceptConnection;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.Domain.RequestConnection;
using Lending.Execution.UnitOfWork;
using Lending.ReadModels.Relational.SearchForUser;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Lending.Execution.WebServices
{
    public class RequestConnectionWebservice : AuthenticatedWebservice<RequestConnection, Result>
    {
        public RequestConnectionWebservice(IUnitOfWork unitOfWork,
            IAuthenticatedMessageHandler<RequestConnection, Result> messageHandler)
            : base(unitOfWork, messageHandler)
        { }

    }

    public class AcceptConnectionWebservice : AuthenticatedWebservice<AcceptConnection, Result>
    {
        public AcceptConnectionWebservice(IUnitOfWork unitOfWork,
            IAuthenticatedMessageHandler<AcceptConnection, Result> messageHandler)
            : base(unitOfWork, messageHandler)
        { }

    }

    public class AddBookToLibraryWebservice : Webservice<AddBookToLibrary, Result>
    {
        public AddBookToLibraryWebservice(IUnitOfWork unitOfWork,
            IAuthenticatedMessageHandler<AddBookToLibrary, Result> messageHandler)
            : base(unitOfWork, messageHandler)
        { }

    }

    public class RemoveBookFromLibraryWebservice : AuthenticatedWebservice<RemoveBookFromLibrary, Result>
    {
        public RemoveBookFromLibraryWebservice(IUnitOfWork unitOfWork,
            IAuthenticatedMessageHandler<RemoveBookFromLibrary, Result> messageHandler)
            : base(unitOfWork, messageHandler)
        { }

    }

    public class SearchForUserWebservice : Webservice<SearchForUser, Result>
    {
        public SearchForUserWebservice(IUnitOfWork unitOfWork,
            IMessageHandler<SearchForUser, Result> messageHandler)
            : base(unitOfWork, messageHandler)
        { }

    }


}
