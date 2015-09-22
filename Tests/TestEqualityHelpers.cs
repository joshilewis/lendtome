using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core;
using Lending.Core.Connect;
using Lending.Core.Model;
using Lending.Core.NewUser;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Is = NUnit.Framework.Is;

namespace Tests
{
    public static class TestEqualityHelpers
    {
        public static bool ShouldEqual(this User actual, User expected)
        {
            Assert.That(actual.UserName, Is.EqualTo(expected.UserName));
            Assert.That(actual.EmailAddress, Is.EqualTo(expected.EmailAddress));

            return true;
        }

        public static User MatchArg(this User expected)
        {
            return Arg<User>.Matches(x => x.ShouldEqual(expected));
        }

        
        public static bool ShouldEqual(this Guid actual, Guid expected)
        {
            Assert.That(actual, Is.EqualTo(expected));

            return true;
        }

        public static Guid MatchArg(this Guid expected)
        {
            return Arg<Guid>.Matches(x => x.ShouldEqual(expected));
        }


        public static bool ShouldEqual(this Guid? actual, Guid? expected)
        {
            if (actual == null)
            {
                Assert.That(expected, Is.Null);
                return true;
            }

            Assert.That(actual, Is.EqualTo(expected));

            return true;
        }

        public static Guid? MatchArg(this Guid? expected)
        {
            return Arg<Guid?>.Matches(x => x.ShouldEqual(expected));
        }


        public static bool ShouldEqual(this BaseResponse actual, BaseResponse expected)
        {
            Assert.That(actual.Success, Is.EqualTo(expected.Success));
            Assert.That(actual.FailureDescription, Is.EqualTo(expected.FailureDescription));

            return true;
        }

        public static BaseResponse MatchArg(this BaseResponse expected)
        {
            return Arg<BaseResponse>.Matches(x => x.ShouldEqual(expected));
        }


        public static bool ShouldEqual(this Connection actual, Connection expected)
        {
            actual.User1.ShouldEqual(expected.User1);
            actual.User2.ShouldEqual(expected.User2);

            return true;
        }

        public static Connection MatchArg(this Connection expected)
        {
            return Arg<Connection>.Matches(x => x.ShouldEqual(expected));
        }


        public static bool ShouldEqual(this Item actual, Item expected)
        {
            Assert.That(actual.Title, Is.EqualTo(expected.Title));
            Assert.That(actual.Creator, Is.EqualTo(expected.Creator));
            Assert.That(actual.Edition, Is.EqualTo(expected.Edition));

            return true;
        }

        public static Item MatchArg(this Item expected)
        {
            return Arg<Item>.Matches(x => x.ShouldEqual(expected));
        }



        public static bool ShouldEqual(this Ownership<User> actual, Ownership<User> expected)
        {
            actual.Item.ShouldEqual(expected.Item);
            actual.Owner.ShouldEqual(expected.Owner);

            return true;
        }

        public static Ownership<User> MatchArg(this Ownership<User> expected)
        {
            return Arg<Ownership<User>>.Matches(x => x.ShouldEqual(expected));
        }


        public static bool ShouldEqual(this Ownership<Organisation> actual, Ownership<Organisation> expected)
        {
            actual.Item.ShouldEqual(expected.Item);
            actual.Owner.ShouldEqual(expected.Owner);

            return true;
        }

        public static Ownership<Organisation> MatchArg(this Ownership<Organisation> expected)
        {
            return Arg<Ownership<Organisation>>.Matches(x => x.ShouldEqual(expected));
        }


        public static bool ShouldEqual(this Organisation actual, Organisation expected)
        {
            Assert.That(actual.Name, Is.EqualTo(expected.Name));

            return true;
        }

        public static Organisation MatchArg(this Organisation expected)
        {
            return Arg<Organisation>.Matches(x => x.ShouldEqual(expected));
        }


        public static bool ShouldEqual(this Borrowing actual, Borrowing expected)
        {
            actual.Borrower.ShouldEqual(expected.Borrower);
            actual.Ownership.ShouldEqual(expected.Ownership);
            actual.Borrower.ShouldEqual(expected.Borrower);

            return true;
        }

        public static Borrowing MatchArg(this Borrowing expected)
        {
            return Arg<Borrowing>.Matches(x => x.ShouldEqual(expected));
        }

        public static bool ShouldEqual(this Ownership actual, Ownership expected)
        {
            actual.Item.ShouldEqual(expected.Item);
            Assert.That(actual.OwnerId, Is.EqualTo(expected.OwnerId));

            return true;
        }

        public static Ownership MatchArg(this Ownership expected)
        {
            return Arg<Ownership>.Matches(x => x.ShouldEqual(expected));
        }

        public static bool ShouldEqual(this UserAdded actual, UserAdded expected)
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.UserName, Is.EqualTo(expected.UserName));
            Assert.That(actual.EmailAddress, Is.EqualTo(expected.EmailAddress));

            return true;
        }

        public static UserAdded MatchArg(this UserAdded expected)
        {
            return Arg<UserAdded>.Matches(x => x.ShouldEqual(expected));
        }

        public static bool ShouldEqual(this ConnectionRequested actual, ConnectionRequested expected)
        {
            Assert.That(actual.FromUserId, Is.EqualTo(expected.FromUserId));
            Assert.That(actual.ToUserId, Is.EqualTo(expected.ToUserId));
            return true;
        }

			
    }
}
