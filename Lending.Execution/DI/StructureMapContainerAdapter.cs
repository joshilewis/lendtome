using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Configuration;
using StructureMap;

namespace Lending.Execution.DI
{
    public class StructureMapContainerAdapter : IContainerAdapter
    {
        public T TryResolve<T>()
        {
            return ObjectFactory.TryGetInstance<T>();
        }

        public T Resolve<T>()
        {
            return ObjectFactory.TryGetInstance<T>();
        }
    }
}
