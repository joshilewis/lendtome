using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Domain;
using Lending.Domain.RequestConnection;
using Lending.Execution.UnitOfWork;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Lending.Execution.WebServices
{
    public class ConnectionRequestWebservice : AuthenticatedWebserviceBase<RequestConnection, Response>
    {
        public ConnectionRequestWebservice(IUnitOfWork unitOfWork,
            IAuthenticatedCommandHandler<RequestConnection, Response> commandHandler)
            : base(unitOfWork, commandHandler)
        { }

    }


}
