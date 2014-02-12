using System;
using Lending.Core;

namespace Lending.Execution.GetUser
{
    public class GetUserRequestResponse : BaseResponse<Guid>
    {
        public GetUserRequestResponse(Guid payload) 
            : base(payload)
        { }

        public GetUserRequestResponse(string failureDescription, Guid payload) 
            : base(failureDescription, payload)
        { }
    }
}
