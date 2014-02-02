using Lending.Execution.UnitOfWork;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class UnitOfWorkAuthProvider : BasicAuthProvider
    {
        private readonly IUnitOfWork unitOfWork;

        public UnitOfWorkAuthProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public override object Authenticate(IServiceBase authService, IAuthSession session, ServiceStack.ServiceInterface.Auth.Auth request)
        {
            object returnValue = null;
            unitOfWork.DoInTransaction(
                () =>
                {
                    returnValue = base.Authenticate(authService, session, request);
                });

            return returnValue;
        }
    }
}
