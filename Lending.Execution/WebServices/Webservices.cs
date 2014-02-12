using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core;
using Lending.Core.AddItem;
using Lending.Core.BorrowItem;
using Lending.Core.Connect;
using Lending.Core.GetUserItems;
using Lending.Core.Model;
using Lending.Execution.UnitOfWork;
using ServiceStack.ServiceInterface;

namespace Lending.Execution.WebServices
{
    public class AddUserItemWebservice : AuthenticatedWebserviceBase<AddUserItemRequest, BaseResponse>, IWebserviceBase<AddUserItemRequest, BaseResponse>
    {
        public AddUserItemWebservice(IUnitOfWork unitOfWork,
            IAuthenticatedRequestHandler<AddUserItemRequest, BaseResponse> requestHandler)
            : base(unitOfWork, requestHandler)
        { }

    }

    public class AddOrgItemWebservice : AuthenticatedWebserviceBase<AddOrganisationItemRequest, BaseResponse>, IWebserviceBase<AddOrganisationItemRequest, BaseResponse>
    {
        public AddOrgItemWebservice(IUnitOfWork unitOfWork,
            IAuthenticatedRequestHandler<AddOrganisationItemRequest, BaseResponse> requestHandler)
            : base(unitOfWork, requestHandler)
        { }

    }

    public class ConnectWebservice : WebserviceBase<ConnectRequest, BaseResponse>, IWebserviceBase<ConnectRequest, BaseResponse>
    {
        public ConnectWebservice(IUnitOfWork unitOfWork,
            IRequestHandler<ConnectRequest, BaseResponse> requestHandler)
            : base(unitOfWork, requestHandler)
        { }

    }

    public class BorrowItemWebservice : WebserviceBase<BorrowItemRequest, BaseResponse>, IWebserviceBase<BorrowItemRequest, BaseResponse>
    {
        public BorrowItemWebservice(IUnitOfWork unitOfWork,
            IRequestHandler<BorrowItemRequest, BaseResponse> requestHandler)
            : base(unitOfWork, requestHandler)
        { }

    }

    public class GetUsersItemsWebservice : AuthenticatedWebserviceBase<GetUserItemsRequest, GetUserItemsRequestResponse>, IWebserviceBase<GetUserItemsRequest, GetUserItemsRequestResponse>
    {
        public GetUsersItemsWebservice(IUnitOfWork unitOfWork,
            IAuthenticatedRequestHandler<GetUserItemsRequest, GetUserItemsRequestResponse> requestHandler)
            : base(unitOfWork, requestHandler)
        { }

    }

}
