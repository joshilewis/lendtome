using System;
using Common.Logging;


namespace Joshilewis.Infrastructure.UnitOfWork
{
    public static class UnitOfWorkHelpers
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UnitOfWorkHelpers).FullName);

        public static void DoInTransaction(this IUnitOfWork uow, Action action)
        {
            uow.Begin();
            try
            {
                action.Invoke();
                uow.Commit();
            }
            catch (Exception ex)
            {
                Log.Error(
                    $"Exception thrown in transaction. Action method: {action.Method}, action Target: {action.Target}.",
                    ex);
                uow.RollBack();
                throw;
            }
        }
    }
}
