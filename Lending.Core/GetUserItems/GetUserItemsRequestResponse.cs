using Lending.Core.Model;

namespace Lending.Core.GetUserItems
{
    public class GetUserItemsRequestResponse : BaseResponse<Ownership[]>
    {
        public GetUserItemsRequestResponse(Ownership[] payload)
            : base(payload)
        { }

        public GetUserItemsRequestResponse(string failureDescription, Ownership[] payload)
            : base(failureDescription, payload)
        { }
    }
}
