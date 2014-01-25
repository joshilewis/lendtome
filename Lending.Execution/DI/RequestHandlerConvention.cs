using System;
using Lending.Core;

namespace Lending.Execution.DI
{
    public class RequestHandlerConvention// : IRegistrationConvention
    {
        private static readonly Type openHandlerInterfaceType = typeof(IRequestHandler<,>);
        
        //public void Process(Type type, Registry registry)
        //{
        //    if (!type.IsAbstract && typeof(Request).IsAssignableFrom(type))
        //    {
        //        Type closedHandlerInterfaceType = openHandlerInterfaceType.MakeGenericType(type);
        //        Type closedDeleteCommandHandlerType = _openDeleteCommandHandlerType.MakeGenericType(type);

        //        registry.For(closedHandlerInterfaceType).Use(closedDeleteCommandHandlerType);
        //    }

        //}
    }
}
