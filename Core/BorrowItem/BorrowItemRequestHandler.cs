using System;
using Core.Model;
using NHibernate;

namespace Core.BorrowItem
{
    public class BorrowItemRequestHandler<T> : IRequestHandler<BorrowItemRequest, BaseResponse> where T : class, IOwner
    {
        private readonly Func<ISession> getSession;

        public BorrowItemRequestHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected BorrowItemRequestHandler() { }

        public virtual BaseResponse HandleRequest(BorrowItemRequest request)
        {
            ISession session = getSession();
            User requestor = session
                .Get<User>(request.RequestorId)
                ;

            Ownership<T> ownership = session
                .Get<Ownership<T>>(request.OwnershipId)
                ;

            var borrowing = new Borrowing(requestor, ownership);

            session.Save(borrowing);

            return new BaseResponse();
        }
    }
}