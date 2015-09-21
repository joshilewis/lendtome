using System;
using System.Collections.Concurrent;
using Lending.Core;
using NHibernate;

namespace Lending.Execution.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Begin();
        void Commit();
        void RollBack();

        ISession CurrentSession { get; }
        ConcurrentQueue<Event> Queue { get; }
    }
}
