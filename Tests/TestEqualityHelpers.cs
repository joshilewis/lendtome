using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain;
using Lending.Domain.AcceptConnection;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.Model;
using Lending.Domain.RegisterUser;
using Lending.Domain.RequestConnection;
using Lending.Execution.Auth;
using Lending.ReadModels.Relational.BookAdded;
using Lending.ReadModels.Relational.ConnectionAccepted;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using Tests.Domain;
using Tests.ReadModels;
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


        public static bool ShouldEqual(this Result actual, Result expected)
        {
            Assert.That(actual.Success, Is.EqualTo(expected.Success));
            Assert.That(actual.Reason, Is.EqualTo(expected.Reason));

            return true;
        }

        public static Result MatchArg(this Result expected)
        {
            return Arg<Result>.Matches(x => x.ShouldEqual(expected));
        }

        public static bool ShouldEqual(this Event actual, Event expected)
        {
            Assert.That(actual.AggregateId, Is.EqualTo(expected.AggregateId));
            Assert.That(actual.ProcessId, Is.EqualTo(expected.ProcessId));
            return true;
        }

        public static bool ShouldEqual(this UserRegistered actual, UserRegistered expected)
        {
            Assert.That(actual.UserName, Is.EqualTo(expected.UserName));
            Assert.That(actual.EmailAddress, Is.EqualTo(expected.EmailAddress));
            ((Event)actual).ShouldEqual(expected);

            return true;
        }

        public static UserRegistered MatchArg(this UserRegistered expected)
        {
            return Arg<UserRegistered>.Matches(x => x.ShouldEqual(expected));
        }

        public static bool ShouldEqual(this ConnectionRequested actual, ConnectionRequested expected)
        {
            Assert.That(actual.TargetUserId, Is.EqualTo(expected.TargetUserId));
            ((Event) actual).ShouldEqual(expected);
            return true;
        }

        public static bool ShouldEqual(this ServiceStackUser actual, ServiceStackUser expected)
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.AuthenticatedUserId, Is.EqualTo(expected.AuthenticatedUserId));
            Assert.That(actual.UserName, Is.EqualTo(expected.UserName));

            return true;
        }

        public static bool ShouldEqual(this RegisteredUser actual, RegisteredUser expected)
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.UserName, Is.EqualTo(expected.UserName));

            return true;
        }

        public static bool ShouldEqual(this ConnectionRequestReceived actual, ConnectionRequestReceived expected)
        {
            Assert.That(actual.RequestingUserId, Is.EqualTo(expected.RequestingUserId));
            ((Event)actual).ShouldEqual(expected);

            return true;
        }

        public static bool ShouldEqual(this ConnectionAccepted actual, ConnectionAccepted expected)
        {
            Assert.That(actual.RequestingUserId, Is.EqualTo(expected.RequestingUserId));
            ((Event)actual).ShouldEqual(expected);

            return true;
        }

        public static bool ShouldEqual(this ConnectionCompleted actual, ConnectionCompleted expected)
        {
            Assert.That(actual.AcceptingUserId, Is.EqualTo(expected.AcceptingUserId));
            ((Event)actual).ShouldEqual(expected);

            return true;
        }

        public static bool ShouldEqual(this BookAddedToLibrary actual, BookAddedToLibrary expected)
        {
            Assert.That(actual.Title, Is.EqualTo(expected.Title));
            Assert.That(actual.Author, Is.EqualTo(expected.Author));
            Assert.That(actual.Isbn, Is.EqualTo(expected.Isbn));
            ((Event)actual).ShouldEqual(expected);

            return true;
        }

        public static bool ShouldEqual(this Result<RegisteredUser[]> actual, Result<RegisteredUser[]> expected)
        {
            Assert.That(actual.Payload, Is.EquivalentTo(expected.Payload).Using(new ValueEqualityComparer()));

            ((Result) actual).ShouldEqual(expected);

            return true;
        }

        public static bool ShouldEqual(this UserConnection actual, UserConnection expected)
        {
            Assert.That(actual.ProcessId, Is.EqualTo(expected.ProcessId));
            Assert.That(actual.RequestingUserId, Is.EqualTo(expected.RequestingUserId));
            Assert.That(actual.AcceptingUserId, Is.EqualTo(expected.AcceptingUserId));
            return true;
        }

        public static bool ShouldEqual(this LibraryBook actual, LibraryBook expected)
        {
            Assert.That(actual.ProcessId, Is.EqualTo(expected.ProcessId));
            Assert.That(actual.OwnerId, Is.EqualTo(expected.OwnerId));
            Assert.That(actual.Title, Is.EqualTo(expected.Title));
            Assert.That(actual.Author, Is.EqualTo(expected.Author));
            Assert.That(actual.Isbn, Is.EqualTo(expected.Isbn));
            return true;
        }



    }

    public class ValueEqualityComparer : IEqualityComparer<RegisteredUser>
    {
        public bool Equals(RegisteredUser x, RegisteredUser y)
        {
            return y.ShouldEqual(x);
        }

        public int GetHashCode(RegisteredUser obj)
        {
            throw new NotImplementedException();
        }
    }
}
