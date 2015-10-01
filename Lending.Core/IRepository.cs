using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Core
{
    public interface IRepository
    {
        TAggregate GetById<TAggregate>(Guid id) where TAggregate : Aggregate;
        TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : Aggregate;
        void Save(Aggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders);
        void EmitEvent(string stream, Event @event);
    }
}
