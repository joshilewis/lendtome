using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core;
using Lending.Core.BorrowItem;
using Lending.Core.Model;
using NUnit.Framework;

namespace Tests.BorrowItem
{
    [TestFixture]
    public class BorrowItemRequestHandlerTests : DatabaseFixtureBase
    {

        [Test]
        public void Test_Success()
        {
            var requestor = new User("requestor", "requestor@email.org");
            var owner = new User("owner", "owner@example.org");
            var item = new Item("title", "creator", "edition");
            var ownership = new Ownership<User>(item, owner);

            SaveEntities(new object[]{requestor, owner, item, ownership});

            CommitTransactionAndOpenNew();

            var request = new BorrowItemRequest(){OwnershipId = ownership.Id, RequestorId = requestor.Id};
            var expectedResponse = new BaseResponse();

            var expectedBorrowing = new Borrowing(requestor, ownership);

            var sut = new BorrowItemRequestHandler(() => Session);
            BaseResponse actualResponse = sut.HandleRequest(request);

            actualResponse.ShouldEqual(expectedResponse);

            //Check that the right Borrowing object is in the DB
            Borrowing borrowingAlias = null;
            User requestorAllias = null;
            Ownership ownershipAlias = null;

            Borrowing borrowingInDb = Session
                .QueryOver<Borrowing>(() => borrowingAlias)
                .JoinAlias(() => borrowingAlias.Borrower, () => requestorAllias)
                .JoinAlias(() => borrowingAlias.Ownership, () => ownershipAlias)
                .Where(() => requestorAllias.Id == requestor.Id)
                .And(() => ownershipAlias.Id == ownership.Id)
                .SingleOrDefault()
                ;

            borrowingInDb.ShouldEqual(expectedBorrowing);

        }
    }
}
