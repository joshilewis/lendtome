using System;

//using ServiceStack.Logging;

namespace Joshilewis.Infrastructure.UnitOfWork
{
    public static class UnitOfWorkHelpers
    {
        //private readonly static ILog Log = LogManager.GetLogger(typeof(UnitOfWorkHelpers).FullName);

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
                //Log.Error(string.Format("Exception thrown in transaction. Action method: {0}, action Target: {1}.",
                //    action.Method, action.Target), ex);
                uow.RollBack();
                throw;
            }
        }
    }
}
