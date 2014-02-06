using Lending.Core.Model;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Execution.Auth
{
    public class ServiceStackUser : User
    {
        public virtual UserAuthPersistenceDto AuthenticatedUser { get; protected set; }

        public ServiceStackUser(UserAuthPersistenceDto userAuth)
        {
            AuthenticatedUser = userAuth;
        }

        protected ServiceStackUser() { }

        public override string UserName
        {
            get { return AuthenticatedUser.FullName ; }
        }

        public override string EmailAddress
        {
            get { return AuthenticatedUser.PrimaryEmail; }
        }
    }
}
