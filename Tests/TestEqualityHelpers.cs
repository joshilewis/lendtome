using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Query;
using Lending.Domain;
using Lending.Domain.AcceptLink;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using Lending.Domain.RequestLink;
using Lending.Execution.Auth;
using Lending.ReadModels.Relational.BookAdded;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.LinkAccepted;
using Lending.ReadModels.Relational.LinkRequested;
using Lending.ReadModels.Relational.SearchForBook;
using NUnit.Framework;
using Rhino.Mocks;
using Is = NUnit.Framework.Is;

namespace Tests
{
    public static class TestEqualityHelpers
    {
        public static bool ShouldEqual(this Library actual, Library expected)
        {
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.Administrators, Is.EquivalentTo(expected.Administrators));

            return true;
        }

        public static Library MatchArg(this Library expected)
        {
            return Arg<Library>.Matches(x => x.ShouldEqual(expected));
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
            Assert.That(actual.Code, Is.EqualTo(expected.Code));

            return true;
        }

        public static Result MatchArg(this Result expected)
        {
            return Arg<Result>.Matches(x => x.ShouldEqual(expected));
        }

        public static bool ShouldEqual(this Event actual, Event expected)
        {
            Assert.That(actual.AggregateId, Is.EqualTo(expected.AggregateId));
            return true;
        }

        public static bool ShouldEqual(this LibraryOpened actual, LibraryOpened expected)
        {
            Assert.That(actual.Name, Is.EqualTo(expected.Name));
            Assert.That(actual.AdministratorId, Is.EqualTo(expected.AdministratorId));
            ((Event)actual).ShouldEqual(expected);

            return true;
        }

        public static LibraryOpened MatchArg(this LibraryOpened expected)
        {
            return Arg<LibraryOpened>.Matches(x => x.ShouldEqual(expected));
        }

        public static bool ShouldEqual(this LinkRequested actual, LinkRequested expected)
        {
            Assert.That(actual.TargetLibraryId, Is.EqualTo(expected.TargetLibraryId));
            ((Event) actual).ShouldEqual(expected);
            return true;
        }

        public static bool ShouldEqual(this OpenedLibrary actual, OpenedLibrary expected)
        {
            Assert.That(actual.AdministratorId, Is.EqualTo(expected.AdministratorId));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));

            return true;
        }

        public static bool ShouldEqual(this LinkRequestReceived actual, LinkRequestReceived expected)
        {
            Assert.That(actual.RequestingLibraryId, Is.EqualTo(expected.RequestingLibraryId));
            ((Event)actual).ShouldEqual(expected);

            return true;
        }

        public static bool ShouldEqual(this LinkAccepted actual, LinkAccepted expected)
        {
            Assert.That(actual.RequestingLibraryId, Is.EqualTo(expected.RequestingLibraryId));
            ((Event)actual).ShouldEqual(expected);

            return true;
        }

        public static bool ShouldEqual(this LinkCompleted actual, LinkCompleted expected)
        {
            Assert.That(actual.AcceptingLibraryId, Is.EqualTo(expected.AcceptingLibraryId));
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

        public static bool ShouldEqual(this Result<OpenedLibrary[]> actual, Result<OpenedLibrary[]> expected)
        {
            Assert.That(actual.Payload,
                Is.EquivalentTo(expected.Payload)
                    .Using((IEqualityComparer<OpenedLibrary>) new ValueEqualityComparer()));

            ((Result) actual).ShouldEqual(expected);

            return true;
        }

        public static bool ShouldEqual(this LibraryLink actual, LibraryLink expected)
        {
            actual.RequestingLibrary.ShouldEqual(expected.RequestingLibrary);
            actual.AcceptingLibrary.ShouldEqual(expected.AcceptingLibrary);

            return true;
        }

        public static bool ShouldEqual(this LibraryBook actual, LibraryBook expected)
        {
            actual.Library.ShouldEqual(expected.Library);
            Assert.That(actual.Title, Is.EqualTo(expected.Title));
            Assert.That(actual.Author, Is.EqualTo(expected.Author));
            Assert.That(actual.Isbn, Is.EqualTo(expected.Isbn));
            return true;
        }
        public static bool ShouldEqual(this LibraryBook[] actual, LibraryBook[] expected)
        {
            Assert.That(actual, Is.EquivalentTo(expected).Using((IEqualityComparer<LibraryBook>)new ValueEqualityComparer()));

            return true;
        }

        public static bool ShouldEqual(this Result<BookSearchResult[]> actual, Result<BookSearchResult[]> expected)
        {
            Assert.That(actual.Payload, Is.EquivalentTo(expected.Payload));

            ((Result)actual).ShouldEqual(expected);

            return true;
        }

        public static bool ShouldEqual(this AuthenticatedUser actual, AuthenticatedUser expected)
        {
            Assert.That(actual.UserName, Is.EqualTo(expected.UserName));
            Assert.That(actual.AuthenticationProviders, Is.EquivalentTo(expected.AuthenticationProviders)
                .Using((IEqualityComparer<AuthenticationProvider>)new ValueEqualityComparer()));

            return true;
        }

        public static bool ShouldEqual(this AuthenticationProvider actual, AuthenticationProvider expected)
        {
            Assert.That(actual.UserId, Is.EqualTo(expected.UserId));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));

            return true;
        }

        public static bool ShouldEqual(this Exception actual, Exception expected)
        {
            Assert.That(actual.GetType(), Is.EqualTo(expected.GetType()));
            Assert.That(actual.Message, Is.EqualTo(expected.Message));
            Assert.That(actual.Data, Is.EqualTo(expected.Data));

            return true;
        }

        public static bool ShouldEqual(this RequestedLink actual, RequestedLink expected)
        {
            actual.RequestingLibrary.ShouldEqual(actual.RequestingLibrary);
            actual.TargetLibrary.ShouldEqual(actual.TargetLibrary);

            return true;
        }

        public static bool ShouldEqual(this RequestedLink[] actual, RequestedLink[] expected)
        {
            Assert.That(actual, Is.EquivalentTo(expected).Using((IEqualityComparer<RequestedLink>) new ValueEqualityComparer()));

            return true;
        }

        public static bool ShouldEqual(this BookSearchResult[] actual, BookSearchResult[] expected)
        {
            Assert.That(actual, Is.EquivalentTo(expected));

            return true;
        }

        public static bool ShouldEqual(this LibraryLink[] actual, LibraryLink[] expected)
        {
            Assert.That(actual, Is.EquivalentTo(expected).Using((IEqualityComparer<LibraryLink>)new ValueEqualityComparer()));

            return true;
        }

        public static bool ShouldEqual(this OpenedLibrary[] actual, OpenedLibrary[] expected)
        {
            Assert.That(actual, Is.EquivalentTo(expected).Using((IEqualityComparer<OpenedLibrary>)new ValueEqualityComparer()));

            return true;
        }

    }

    public class ValueEqualityComparer : IEqualityComparer<OpenedLibrary>,
        IEqualityComparer<AuthenticationProvider>,
        IEqualityComparer<LibraryLink>,
        IEqualityComparer<RequestedLink>,
        IEqualityComparer<LibraryBook>
    {
        public bool Equals(OpenedLibrary x, OpenedLibrary y)
        {
            return y.ShouldEqual(x);
        }

        public int GetHashCode(OpenedLibrary obj)
        {
            throw new NotImplementedException();
        }

        public bool Equals(AuthenticationProvider x, AuthenticationProvider y)
        {
            return y.ShouldEqual(x);
        }

        public int GetHashCode(AuthenticationProvider obj)
        {
            throw new NotImplementedException();
        }

        public bool Equals(LibraryOpened x, LibraryOpened y)
        {
            return y.ShouldEqual(x);
        }

        public int GetHashCode(LibraryOpened obj)
        {
            throw new NotImplementedException();
        }

        public bool Equals(LinkRequested x, LinkRequested y)
        {
            return y.ShouldEqual(x);
        }

        public int GetHashCode(LinkRequested obj)
        {
            throw new NotImplementedException();
        }

        public bool Equals(LinkRequestReceived x, LinkRequestReceived y)
        {
            return y.ShouldEqual(x);
        }

        public int GetHashCode(LinkRequestReceived obj)
        {
            throw new NotImplementedException();
        }

        public bool Equals(LibraryLink x, LibraryLink y)
        {
            return y.ShouldEqual(x);
        }

        public int GetHashCode(LibraryLink obj)
        {
            throw new NotImplementedException();
        }

        public bool Equals(RequestedLink x, RequestedLink y)
        {
            return y.ShouldEqual(x);
        }

        public int GetHashCode(RequestedLink obj)
        {
            throw new NotImplementedException();
        }

        public bool Equals(LibraryBook x, LibraryBook y)
        {
            return y.ShouldEqual(x);
        }

        public int GetHashCode(LibraryBook obj)
        {
            throw new NotImplementedException();
        }
    }
}
