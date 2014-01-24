using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.AddUser;
using Core.Model;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests
{
    public static class TestEqualityHelpers
    {
        public static bool ShouldEqual(this User actual, User expected, Guid userId)
        {
            Assert.That(actual.Id, Is.EqualTo(userId));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.EmailAddress, Is.EqualTo(expected.EmailAddress));

            return true;
        }

        public static User MatchArg(this User expected, Guid userId)
        {
            return Arg<User>.Matches(x => x.ShouldEqual(expected, userId));
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
			
			
    }
}
