using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.ReadModels.Relational.ListLibrayLinks;
using NUnit.Framework;
using static Joshilewis.Testing.Helpers.ApiExtensions;
using static Tests.AutomationExtensions;

namespace Tests.Queries
{
    [TestFixture]
    public class LinkRequestSpecs : Fixture
    {
        private Guid transactionId;
        private string user1Id;
        private string user2Id;
        private string user3Id;
        private string user4Id;
        private string user5Id;
        private string user6Id;
        private Guid library1Id;
        private Guid library2Id;
        private Guid library3Id;
        private Guid library4Id;
        private Guid library5Id;
        private Guid library6Id;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            transactionId = Guid.Empty;
            user1Id = "user1Id";
            user2Id = "user2Id";
            user3Id = "user3Id";
            user4Id = "user4Id";
            user5Id = "user5Id";
            user6Id = "user6Id";
        }

        private void LinksAreRequested()
        {
            library1Id = OpenLibrary(transactionId, user1Id, "library1", "library1Picture");
            library2Id = OpenLibrary(transactionId, user2Id, "library2", "library2Picture");
            library3Id = OpenLibrary(transactionId, user3Id, "library3", "library3Picture");
            library4Id = OpenLibrary(transactionId, user4Id, "library4", "library4Picture");
            library5Id = OpenLibrary(transactionId, user5Id, "library5", "library5Picture");
            library6Id = OpenLibrary(transactionId, user6Id, "library6", "library6Picture");
            RequestsLibraryLink(transactionId, library1Id, user1Id, library2Id);
            RequestsLibraryLink(transactionId, library1Id, user1Id, library3Id);
            RequestsLibraryLink(transactionId, library1Id, user1Id, library4Id);
            RequestsLibraryLink(transactionId, library5Id, user5Id, library1Id);
            RequestsLibraryLink(transactionId, library5Id, user5Id, library2Id);
        }

        private void LinksAreAccepted()
        {
            AcceptsLibraryLink(transactionId, library3Id, user3Id, library1Id);
            AcceptsLibraryLink(transactionId, library2Id, user2Id, library1Id);
            AcceptsLibraryLink(transactionId, library2Id, user2Id, library5Id);
        }

        [Test]
        public void CorrectSentLinksReturnedForUser1()
        {
            LinksAreRequested();

            GetEndpoint($"/libraries/{library1Id}/links/sent").As(user1Id);

            ResponseIs(
                new LibrarySearchResult(library2Id, "library2", "library2Picture"),
                new LibrarySearchResult(library3Id, "library3", "library3Picture"),
                new LibrarySearchResult(library4Id, "library4", "library4Picture")
            );
        }

        [Test]
        public void CorrectSentLinksReturnedForUser2()
        {

            LinksAreRequested();

            GetEndpoint($"/libraries/{library2Id}/links/sent").As(user2Id);

            ResponseIs(new LibrarySearchResult[] { });

        }

        [Test]
        public void CorrectSentLinksReturnedForUser5()
        {
            LinksAreRequested();

            GetEndpoint($"/libraries/{library5Id}/links/sent").As(user5Id);

            ResponseIs(
                new LibrarySearchResult(library1Id, "library1", "library1Picture"),
                new LibrarySearchResult(library2Id, "library2", "library2Picture")
            );
        }

        [Test]
        public void CorrectRecievedLinksReturnedForUser1()
        {
            LinksAreRequested();

            GetEndpoint($"/libraries/{library1Id}/links/received").As(user1Id);

            ResponseIs(new LibrarySearchResult(library5Id, "library5", "library5Picture"));
        }

        [Test]
        public void CorrectRecievedLinksReturnedForUser2()
        {

            LinksAreRequested();

            GetEndpoint($"/libraries/{library2Id}/links/received").As(user2Id);

            ResponseIs(
                new LibrarySearchResult(library1Id, "library1", "library1Picture"),
                new LibrarySearchResult(library5Id, "library5", "library5Picture")
            );
        }

        [Test]
        public void CorrectRecievedLinksReturnedForUser5()
        {
            LinksAreRequested();

            GetEndpoint($"/libraries/{library5Id}/links/received").As(user5Id);

            ResponseIs(new LibrarySearchResult[] { });
        }

        [Test]
        public void LinksReturnedForUser1()
        {
            LinksAreRequested();
            LinksAreAccepted();

            GetEndpoint($"/libraries/{library1Id}/links/").As(user1Id);

            ResponseIs(
                new LibrarySearchResult(library2Id, "library2", "library2Picture"),
                new LibrarySearchResult(library3Id, "library3", "library3Picture")
            );
        }

        [Test]
        public void LinksReturnedForUser4()
        {
            LinksAreRequested();
            LinksAreAccepted();

            GetEndpoint($"/libraries/{library4Id}/links/").As(user4Id);

            ResponseIs(new LibrarySearchResult[] { });
        }

        [Test]
        public void LinksReturnedForUser5()
        {
            LinksAreRequested();
            LinksAreAccepted();

            GetEndpoint($"/libraries/{library5Id}/links/").As(user5Id);

            ResponseIs(new LibrarySearchResult(library2Id, "library2", "library2Picture"));
        }

    }
}
