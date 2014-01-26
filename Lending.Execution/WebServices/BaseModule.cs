using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core;
using Nancy;

namespace Lending.Execution.WebServices
{
    public class BaseModule<TRequest, TResponse> : NancyModule
    {
        private readonly IRequestHandler<TRequest, TResponse> requestHandler;

        public BaseModule(IRequestHandler<TRequest, TResponse> requestHandler)
        {
            this.requestHandler = requestHandler;


        }


    }
}
