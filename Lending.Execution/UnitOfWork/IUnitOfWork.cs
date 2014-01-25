using System;
using NHibernate;

namespace Lending.Execution.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Begin();
        void Commit();
        void RollBack();

        ISession CurrentSession { get; }
    }
}
