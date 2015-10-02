using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core;
using Lending.Core.ConnectionRequest;
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
