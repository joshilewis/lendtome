using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests.Smoke
{
    [Category("smoke")]
    [TestFixture]
    public class SmokeTests
    {
        private readonly string baseUri = ConfigurationManager.AppSettings["smoke:url"];
        private static readonly HttpClient client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = false,
        });

        [SetUp]
        public void SetUp()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        [Test]
        public void LibrarySearch()
        {
            var result = client.GetAsync(baseUri + "api/libraries/blah").Result;
            Assert.That(result.IsSuccessStatusCode, Is.True);
            string content = result.Content.ReadAsStringAsync().Result;
            Assert.That(content, Does.StartWith("["));
            Assert.That(content, Does.EndWith("]"));
        }

        [Test]
        public void ListLibraries()
        {
            var result = client.GetAsync(baseUri + "api/libraries/").Result;
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            string content = result.Content.ReadAsStringAsync().Result;
            Assert.That(content, Is.Empty);
        }

        [Test]
        public void SearchForBooks()
        {
            var result = client.GetAsync(baseUri + "api/books/Extreme Programming Explained/").Result;
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            string content = result.Content.ReadAsStringAsync().Result;
            Assert.That(content, Is.Empty);
        }

    }
}
