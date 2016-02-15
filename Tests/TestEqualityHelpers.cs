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
using Lending.ReadModels.Relational.ListLibrayLinks;
using Lending.ReadModels.Relational.SearchForBook;
using NUnit.Framework;
using Rhino.Mocks;
using Is = NUnit.Framework.Is;

namespace Tests
{
    public static class TestEqualityHelpers
    {
        public static bool ShouldEqual(this OpenedLibrary actual, OpenedLibrary expected)
        {
            Assert.That(actual.AdministratorId, Is.EqualTo(expected.AdministratorId));
            Assert.That(actual.Name, Is.EqualTo(expected.Name));

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

        public static bool ShouldEqual(this RequestedLink actual, RequestedLink expected)
        {
            actual.RequestingLibrary.ShouldEqual(actual.RequestingLibrary);
            actual.TargetLibrary.ShouldEqual(actual.TargetLibrary);

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
