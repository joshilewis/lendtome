using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Domain;
using Lending.Domain.AcceptConnection;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.RequestConnection;
using Lending.Execution.UnitOfWork;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Lending.Execution.WebServices
{
    public class RequestConnectionWebservice : AuthenticatedWebserviceBase<RequestConnection, Result>
    {
        public RequestConnectionWebservice(IUnitOfWork unitOfWork,
            IAuthenticatedCommandHandler<RequestConnection, Result> commandHandler)
            : base(unitOfWork, commandHandler)
        { }

    }

    public class AcceptConnectionWebservice : AuthenticatedWebserviceBase<AcceptConnection, Result>
    {
        public AcceptConnectionWebservice(IUnitOfWork unitOfWork,
            IAuthenticatedCommandHandler<AcceptConnection, Result> commandHandler)
            : base(unitOfWork, commandHandler)
        { }

    }

    public class AddBookToCollectionWebservice : AuthenticatedWebserviceBase<AddBookToLibrary, Result>
    {
        public AddBookToCollectionWebservice(IUnitOfWork unitOfWork,
            IAuthenticatedCommandHandler<AddBookToLibrary, Result> commandHandler)
            : base(unitOfWork, commandHandler)
        { }

    }


}
