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
        private readonly List<object> changes;

        public abstract Guid Id { get; protected set; }
        public int Version { get; protected set; }

        protected Aggregate()
        {
            Version = 0;
            changes = new List<object>();
        }

        protected void ApplyEvent(object @event)
        {
            When(@event);
            Version++;
        }

        protected void RaiseEvent(Event @event)
        {
            When(@event);
            changes.Add(@event);
            Version++;
        }

        protected abstract void When(object @event);

        public void ClearUncommittedEvents()
        {
            changes.Clear();
        }

        public ICollection GetUncommittedEvents()
        {
            return changes;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Aggregate)) return false;
            return Id.Equals(((Aggregate) obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
