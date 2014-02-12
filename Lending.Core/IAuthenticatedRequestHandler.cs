using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Core
{
    public interface IAuthenticatedRequestHandler<in TRequest, out TResponse>
    {
        TResponse HandleRequest(TRequest request, int userId);
    }
}
