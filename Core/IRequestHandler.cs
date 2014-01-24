using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IRequestHandler<in TRequest, out TResponse>
    {
        TResponse HandleRequest(TRequest request);
    }
}
