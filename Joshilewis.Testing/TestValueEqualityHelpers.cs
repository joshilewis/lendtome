using System;
using System.Collections.Generic;
using System.Net.Http;
using NUnit.Framework;

namespace Joshilewis.Testing
{
    public static class TestValueEqualityHelpers
    {
        public static bool ShouldEqual(this HttpResponseMessage actual, HttpResponseMessage expected)
        {
            Assert.That(actual.StatusCode, Is.EqualTo(expected.StatusCode));
            Assert.That(actual.ReasonPhrase, Is.EqualTo(expected.ReasonPhrase));

            return true;
        }


    }

}