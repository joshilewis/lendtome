using System;
using Lending.Core.Model;
using NHibernate;

namespace Lending.Core.BorrowItem
{
    public class BorrowItemRequestHandler : IRequestHandler<BorrowItemRequest, BaseResponse>
    {
        private readonly Func<ISession> getSession;

        public BorrowItemRequestHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected BorrowItemRequestHandler() { }

        public virtual BaseResponse HandleRequest(BorrowItemRequest userAuthIdString)
        {
            ISession session = getSession();
            User requestor = session
                .Get<User>(userAuthIdString.RequestorId)
                ;

            Ownership ownership = session
                .Get<Ownership>(userAuthIdString.OwnershipId)
                ;

            var borrowing = new Borrowing(requestor, ownership);

            session.Save(borrowing);

            return new BaseResponse();
        }
    }
}