using System;
using System.Collections.Concurrent;
using Joshilewis.Cqrs;
using Lending.Domain;
using NHibernate;

namespace Lending.Execution.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Begin();
        void Commit();
        void RollBack();

        ISession CurrentSession { get; }
        IEventRepository EventRepository { get; }
    }
}
