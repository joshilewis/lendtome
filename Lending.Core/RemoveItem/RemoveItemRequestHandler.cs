using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core.Model;
using NHibernate;

namespace Lending.Core.RemoveItem
{
    public class RemoveItemRequestHandler : BaseAuthenticatedRequestHandler<RemoveItemRequest, object>
    {
        public RemoveItemRequestHandler(Func<ISession> sessionFunc)
             : base(sessionFunc)
        { }

        protected RemoveItemRequestHandler() { }

        public override object HandleRequest(RemoveItemRequest request, int userId)
        {
            Ownership ownership = Session.Get<Ownership>(request.OwnershipId);
            Session.Delete(ownership);

            return null;
        }
    }
}
