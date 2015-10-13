using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Domain;
using Lending.Domain.AcceptConnection;
using Lending.Domain.RequestConnection;
using Lending.Execution.UnitOfWork;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Lending.Execution.WebServices
{
    public class RequestConnectionWebservice : AuthenticatedWebserviceBase<RequestConnection, Response>
    {
        public RequestConnectionWebservice(IUnitOfWork unitOfWork,
            IAuthenticatedCommandHandler<RequestConnection, Response> commandHandler)
            : base(unitOfWork, commandHandler)
        { }

    }

    public class AcceptConnectionWebservice : AuthenticatedWebserviceBase<AcceptConnection, Response>
    {
        public AcceptConnectionWebservice(IUnitOfWork unitOfWork,
            IAuthenticatedCommandHandler<AcceptConnection, Response> commandHandler)
            : base(unitOfWork, commandHandler)
        { }

    }


}
