using System;
using System.Collections.Generic;
using Lending.ReadModels.Relational;
using NUnit.Framework;
using Is = NUnit.Framework.Is;

namespace Tests
{
    public static class TestEqualityHelpers
    {
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

    }

    public class ValueEqualityComparer : IEqualityComparer<AuthenticationProvider>
    {
        public bool Equals(AuthenticationProvider x, AuthenticationProvider y)
        {
            return y.ShouldEqual(x);
        }

        public int GetHashCode(AuthenticationProvider obj)
        {
            throw new NotImplementedException();
        }

    }
}
