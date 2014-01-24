using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.AddUser;
using Core.ConnectRequest;
using Core.Model;
using NUnit.Framework;
using Rhino.Mocks;

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

        
        public static bool ShouldEqual(this AddUserRequest actual, AddUserRequest expected)
        {
            Assert.That(actual.UserName, Is.EqualTo(expected.UserName));
            Assert.That(actual.EmailAddress, Is.EqualTo(expected.EmailAddress));

            return true;
        }

        public static AddUserRequest MatchArg(this AddUserRequest expected)
        {
            return Arg<AddUserRequest>.Matches(x => x.ShouldEqual(expected));
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


        public static bool ShouldEqual(this AddUserResponse actual, AddUserResponse expected)
        {
            Assert.That(actual.Success, Is.EqualTo(expected.Success));
            Assert.That(actual.FailureDescription, Is.EqualTo(expected.FailureDescription));

            return true;
        }

        public static AddUserResponse MatchArg(this AddUserResponse expected)
        {
            return Arg<AddUserResponse>.Matches(x => x.ShouldEqual(expected));
        }


        public static bool ShouldEqual(this ConnectResponse actual, ConnectResponse expected)
        {
            Assert.That(actual.Success, Is.EqualTo(expected.Success));
            Assert.That(actual.FailureDescription, Is.EqualTo(expected.FailureDescription));

            return true;
        }

        public static ConnectResponse MatchArg(this ConnectResponse expected)
        {
            return Arg<ConnectResponse>.Matches(x => x.ShouldEqual(expected));
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
			
    }
}
