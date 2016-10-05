using System;
using Joshilewis.Cqrs;
using NHibernate;

namespace Joshilewis.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Begin();
        void Commit();
        void RollBack();
    }




}
