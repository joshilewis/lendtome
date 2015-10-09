using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Domain;
using Lending.Domain.ConnectionRequest;
using Lending.Execution.UnitOfWork;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Lending.Execution.WebServices
{
    public class ConnectionRequestWebservice : AuthenticatedWebserviceBase<ConnectionRequest, BaseResponse>
    {
        public ConnectionRequestWebservice(IUnitOfWork unitOfWork,
            IAuthenticatedRequestHandler<ConnectionRequest, BaseResponse> requestHandler)
            : base(unitOfWork, requestHandler)
        { }

    }


}
