using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.AddItem;
using Core.Model;
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
}
