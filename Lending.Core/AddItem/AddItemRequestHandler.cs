using System;
using Lending.Core.Model;
using NHibernate;

namespace Lending.Core.AddItem
{
    public class AddItemRequestHandler<T> : IRequestHandler<AddItemRequest<T>, BaseResponse> where T : class, IOwner
    {
        public const string OwnershipAlreadyExists = "This owner already owns this item.";

        private readonly Func<ISession> getSession;

        public AddItemRequestHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected AddItemRequestHandler() { }

        public virtual BaseResponse HandleRequest(AddItemRequest<T> request)
        {
            Item item = GetItem(request);

            if (item != null && CheckIfItemAlreadyOwned<T>(item.Id, request.OwnerId))
                return new BaseResponse(OwnershipAlreadyExists);

            if (item == null)
                item = CreateItem(request);

            T owner = GetOwner(request);

            var ownership = new Ownership<T>(item, owner);

            getSession().Save(ownership);

            return new BaseResponse();
        }

        private bool CheckIfItemAlreadyOwned<T>(Guid itemId, Guid ownerId) where T : class, IOwner
        {
            Ownership<T> ownershipAlias = null;
            Item itemAlis = null;
            T ownerAlias = null;

            Ownership<T> ownership = getSession()
                .QueryOver<Ownership<T>>(() => ownershipAlias)
                .JoinQueryOver<T>(() => ownershipAlias.Owner, () => ownerAlias)
                .JoinQueryOver<Item>(() => ownershipAlias.Item, () => itemAlis)
                .Where(() => ownerAlias.Id == ownerId)
                .Where(() => itemAlis.Id == itemId)
                .SingleOrDefault()
                ;

            return ownership != null;



        }

        private Item CreateItem(AddItemRequest<T> request)
        {
            var item = new Item(request.Title, request.Creator, request.Edition);
            getSession().Save(item);
            return item;
        }

        protected virtual T GetOwner(AddItemRequest<T> request)
        {
            return getSession()
                .Get<T>(request.OwnerId)
                ;
        }

        protected Item GetItem(AddItemRequest<T> request)
        {
            ISession session = getSession();

            Item item = session
                .QueryOver<Item>()
                .Where(x => x.Title == request.Title)
                .Where(x => x.Creator == request.Creator)
                .Where(x => x.Edition == request.Edition)
                .SingleOrDefault()
                ;

            return item;
        }

        public const string UsernameTaken = "That user name is already in use.";
        public const string EmailTaken = "That Email Address is already in use.";
    }
}