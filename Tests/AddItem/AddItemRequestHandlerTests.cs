using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Model;
using NHibernate;
using NUnit.Framework;

namespace Tests.AddItem
{
    public class AddItemRequestHandlerTests : DatabaseFixtureBase
    {

        [Test]
        public void Test_UserOwner_ItemExists_Success()
        {
            var user = new User("username", "emailAddress");
            var item = new Item("Title", "Creator", "Edition");
            SaveEntities(new object[] { user, item});

            CommitTransactionAndOpenNew();

            var request = new AddUserItemRequest()
            {
                OwnerId = user.Id,
                Creator = item.Creator,
                Edition = item.Edition,
                Title = item.Title
            };

            var expectedResponse = new ResponseBase();

            var expectedOwnership = new Ownership<User>(item, user);

            var sut = new AddItemRequestHandler<User>(() => Session);
            ResponseBase actualResponse = sut.HandleRequest(request);

            actualResponse.ShouldEqual(expectedResponse);

            CommitTransactionAndOpenNew();

            Ownership<User> ownershipInDb = Session
                .QueryOver<Ownership>()
                .JoinQueryOver<Item>(x => x.Item)
                .Where(x => x.Id == item.Id)
                .SingleOrDefault<Ownership<User>>()
                ;

            ownershipInDb.ShouldEqual(expectedOwnership);
        }

        [Test]
        public void Test_OrgOwner_ItemExists_Success()
        {
            var organisation = new Organisation("organisation");
            var item = new Item("Title", "Creator", "Edition");
            SaveEntities(new object[] { organisation, item });

            CommitTransactionAndOpenNew();

            var request = new AddOrganisationItemRequest()
            {
                OwnerId = organisation.Id,
                Creator = item.Creator,
                Edition = item.Edition,
                Title = item.Title
            };

            var expectedResponse = new ResponseBase();

            var expectedOwnership = new Ownership<Organisation>(item, organisation);

            var sut = new AddItemRequestHandler<Organisation>(() => Session);
            ResponseBase actualResponse = sut.HandleRequest(request);

            actualResponse.ShouldEqual(expectedResponse);

            CommitTransactionAndOpenNew();

            Ownership<Organisation> ownershipInDb = Session
                .QueryOver<Ownership>()
                .JoinQueryOver<Item>(x => x.Item)
                .Where(x => x.Id == item.Id)
                .SingleOrDefault<Ownership<Organisation>>()
                ;

            ownershipInDb.ShouldEqual(expectedOwnership);
        }

        [Test]
        public void Test_UserOwner_NewItem_Success()
        {
            var user = new User("username", "emailAddress");
            var expectedItem = new Item("Title", "Creator", "Edition");
            SaveEntities(new object[] { user });

            CommitTransactionAndOpenNew();

            var request = new AddUserItemRequest()
            {
                OwnerId = user.Id,
                Creator = expectedItem.Creator,
                Edition = expectedItem.Edition,
                Title = expectedItem.Title
            };

            var expectedResponse = new ResponseBase();

            var expectedOwnership = new Ownership<User>(expectedItem, user);

            var sut = new AddItemRequestHandler<User>(() => Session);
            ResponseBase actualResponse = sut.HandleRequest(request);

            actualResponse.ShouldEqual(expectedResponse);

            CommitTransactionAndOpenNew();

            Item itemInDb = Session
                .QueryOver<Item>()
                .Where(x => x.Title == expectedItem.Title)
                .Where(x => x.Creator == expectedItem.Creator)
                .Where(x => x.Edition == expectedItem.Edition)
                .SingleOrDefault()
                ;

            itemInDb.ShouldEqual(expectedItem);

            Ownership<User> ownershipInDb = Session
                .QueryOver<Ownership>()
                .JoinQueryOver<Item>(x => x.Item)
                .Where(x => x.Id == itemInDb.Id)
                .SingleOrDefault<Ownership<User>>()
                ;

            ownershipInDb.ShouldEqual(expectedOwnership);
        }

        [Test]
        public void Test_UserOwner_AlreadyOwned()
        {
            var user = new User("username", "emailAddress");
            var item = new Item("Title", "Creator", "Edition");
            var expectedOwnership = new Ownership<User>(item, user);
            SaveEntities(new object[] { user, item, expectedOwnership});

            CommitTransactionAndOpenNew();

            var request = new AddUserItemRequest()
            {
                OwnerId = user.Id,
                Creator = item.Creator,
                Edition = item.Edition,
                Title = item.Title
            };

            var expectedResponse = new ResponseBase(AddItemRequestHandler<User>.OwnershipAlreadyExists);

            var sut = new AddItemRequestHandler<User>(() => Session);
            ResponseBase actualResponse = sut.HandleRequest(request);

            actualResponse.ShouldEqual(expectedResponse);

            CommitTransactionAndOpenNew();

            Ownership<User> ownershipInDb = Session
                .QueryOver<Ownership>()
                .JoinQueryOver<Item>(x => x.Item)
                .Where(x => x.Id == item.Id)
                .SingleOrDefault<Ownership<User>>()
                ;

            ownershipInDb.ShouldEqual(expectedOwnership);
        }
    }

    public class AddItemRequestHandler<T> : IRequestHandler<AddItemRequest<T>, ResponseBase> where T : class, IOwner
    {
        public const string OwnershipAlreadyExists = "This owner already owns this item.";

        private readonly Func<ISession> getSession;

        public AddItemRequestHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected AddItemRequestHandler() { }

        public virtual ResponseBase HandleRequest(AddItemRequest<T> request)
        {
            Item item = GetItem(request);

            if (item != null && CheckIfItemAlreadyOwned<T>(item.Id, request.OwnerId))
                return new ResponseBase(OwnershipAlreadyExists);

            if (item == null)
                item = CreateItem(request);

            T owner = GetOwner(request);

            var ownership = new Ownership<T>(item, owner);

            getSession().Save(ownership);

            return new ResponseBase();
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

    }

    public abstract class AddItemRequest<T> where T : IOwner
    {
        public string Title { get; set; }
        public string Creator { get; set; }
        public string Edition { get; set; }
        public abstract Guid OwnerId {get; set; }
    }

    public class AddUserItemRequest : AddItemRequest<User>
    {
        public override Guid OwnerId { get; set; }

    }

    public class AddOrganisationItemRequest : AddItemRequest<Organisation>
    {
        public override Guid OwnerId { get; set; }

    }

}
