using System;
using System.Collections.Generic;

namespace Lending.Cqrs
{
    public interface IRepository
    {
        IEnumerable<Event> GetEventsForAggregate<TAggregate>(Guid id) where TAggregate : Aggregate;
        IEnumerable<Event> GetEventsForAggregate<TAggregate>(Guid id, int version) where TAggregate : Aggregate;
        void Save(Aggregate aggregate);
    }
}
