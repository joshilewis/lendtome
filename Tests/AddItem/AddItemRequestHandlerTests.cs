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

    }

    public class AddItemRequestHandler<T> : IRequestHandler<AddItemRequest<T>, ResponseBase> where T : IOwner
    {
        protected readonly Func<ISession> GetSession;

        public AddItemRequestHandler(Func<ISession> sessionFunc)
        {
            this.GetSession = sessionFunc;
        }

        protected AddItemRequestHandler() { }

        public virtual ResponseBase HandleRequest(AddItemRequest<T> request)
        {
            Item item = GetItem(request);

            T owner = GetOwner(request);

            var ownership = new Ownership<T>(item, owner);

            GetSession().Save(ownership);

            return new ResponseBase();
        }

        protected virtual T GetOwner(AddItemRequest<T> request)
        {
            return GetSession()
                .Get<T>(request.OwnerId)
                ;
        }

        protected Item GetItem(AddItemRequest<T> request)
        {
            ISession session = GetSession();

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
