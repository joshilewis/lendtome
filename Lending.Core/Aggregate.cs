using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Core
{
    public abstract class Aggregate
    {
        public abstract Guid Id { get; }
        public abstract int Version { get; }
        public abstract void ApplyEvent(object @event);
        public abstract void ClearUncommittedEvents();
        public abstract ICollection GetUncommittedEvents();
    }
}
