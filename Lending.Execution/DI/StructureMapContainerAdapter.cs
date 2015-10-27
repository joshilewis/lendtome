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

        private readonly IContainer container;

        public StructureMapContainerAdapter(IContainer container)
        {
            this.container = container;
        }

        public T TryResolve<T>()
        {
            return container.TryGetInstance<T>();
        }

        public T Resolve<T>()
        {
            return container.TryGetInstance<T>();
        }
    }
}
