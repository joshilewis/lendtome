using System;
using Lending.Core.Model;
using NHibernate;

namespace Lending.Core.AddItem
{
    public class AddUserItemRequestHandler : AddItemRequestHandler<User>
    {
        public AddUserItemRequestHandler(Func<ISession> sessionFunc)
            : base(sessionFunc)
        { }

        protected AddUserItemRequestHandler() { }

        public override BaseResponse HandleRequest(AddItemRequest<User> request, int userId)
        {
            request.OwnerId = userId;
            return base.HandleRequest(request, userId);
        }

    }
}