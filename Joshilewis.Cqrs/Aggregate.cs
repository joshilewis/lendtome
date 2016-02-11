using System;
using System.Collections.Generic;
using System.Linq;
using Joshilewis.Cqrs.Query;

namespace Joshilewis.Cqrs
{
    public abstract class Aggregate
    {
        private readonly List<Event> changes;

        public Guid Id { get; protected set; }
        public int Version { get; protected set; }

        protected Aggregate()
        {
            Version = 0;
            changes = new List<Event>();
        }

        protected void ApplyEvent(Event @event)
        {
            DispatchEvent(@event);
            Version++;
        }

        protected void RaiseEvent(Event @event)
        {
            DispatchEvent(@event);
            Version++;
            changes.Add(@event);
        }

        protected abstract List<IEventRoute> EventRoutes { get; }
        private void DispatchEvent(Event @event)
        {
            foreach (var handler in EventRoutes.Where(x => x.HandlerType == @event.GetType()))
            {
                handler.Handle(@event);
            }
            
        }

        protected virtual void When(Event @event)
        {
            //Default handler, do nothing
        }

        public void ClearUncommittedEvents()
        {
            changes.Clear();
        }

        public IList<Event> GetUncommittedEvents()
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

        protected virtual Result Success()
        {
            return new Result(Result.EResultCode.Ok);
        }

        protected virtual Result Created()
        {
            return new Result(Result.EResultCode.Created);
        }

        protected virtual Result Fail(string reason)
        {
            throw new InvalidOperationException(reason);
        }

    }
}
