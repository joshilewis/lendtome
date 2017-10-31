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
        private Guid user1Id;
        private Guid user2Id;
        private Guid user3Id;
        private Guid user4Id;
        private Guid user5Id;
        private Guid user6Id;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            transactionId = Guid.Empty;
            user1Id = Guid.NewGuid();
            user2Id = Guid.NewGuid();
            user3Id = Guid.NewGuid();
            user4Id = Guid.NewGuid();
            user5Id = Guid.NewGuid();
            user6Id = Guid.NewGuid();

        }

        private void LinksAreRequested()
        {
            UserRegisters(user1Id, "user1", "email1", "user1Picture");
            UserRegisters(user2Id, "user2", "email2", "user2Picture");
            UserRegisters(user3Id, "user3", "email3", "user3Picture");
            UserRegisters(user4Id, "user4", "email4", "user4Picture");
            UserRegisters(user5Id, "user5", "email5", "user5Picture");
            UserRegisters(user6Id, "user6", "email6", "user6Picture");
            OpensLibrary(transactionId, user1Id, "library1");
            OpensLibrary(transactionId, user2Id, "library2");
            OpensLibrary(transactionId, user3Id, "library3");
            OpensLibrary(transactionId, user4Id, "library4");
            OpensLibrary(transactionId, user5Id, "library5");
            OpensLibrary(transactionId, user6Id, "library6");
            RequestsLibraryLink(transactionId, user1Id, user1Id, user2Id);
            RequestsLibraryLink(transactionId, user1Id, user1Id, user3Id);
            RequestsLibraryLink(transactionId, user1Id, user1Id, user4Id);
            RequestsLibraryLink(transactionId, user5Id, user5Id, user1Id);
            RequestsLibraryLink(transactionId, user5Id, user5Id, user2Id);
        }

        private void LinksAreAccepted()
        {
            AcceptsLibraryLink(transactionId, user3Id, user3Id, user1Id);
            AcceptsLibraryLink(transactionId, user2Id, user2Id, user1Id);
            AcceptsLibraryLink(transactionId, user2Id, user2Id, user5Id);
        }

        [Test]
        public void CorrectSentLinksReturnedForUser1()
        {
            Runner.RunScenario(
                given => LinksAreRequested(),

                when => GetEndpoint($"/libraries/{user1Id}/links/sent").As(user1Id),

                then => ResponseIs(
                    new LibrarySearchResult(user2Id, "library2", "user2Picture"),
                    new LibrarySearchResult(user3Id, "library3", "user3Picture"),
                    new LibrarySearchResult(user4Id, "library4", "user4Picture")
                    )
                );
        }

        [Test]
        public void CorrectSentLinksReturnedForUser2()
        {

            Runner.RunScenario(
                given => LinksAreRequested(),

                when => GetEndpoint($"/libraries/{user2Id}/links/sent").As(user2Id),

                then => ResponseIs(new LibrarySearchResult[] { })
            );

        }

        [Test]
        public void CorrectSentLinksReturnedForUser5()
        {

            Runner.RunScenario(
                given => LinksAreRequested(),

                when => GetEndpoint($"/libraries/{user5Id}/links/sent").As(user5Id),

                then => ResponseIs(
                    new LibrarySearchResult(user1Id, "library1", "user1Picture"),
                    new LibrarySearchResult(user2Id, "library2", "user2Picture")
                )
            );
        }

        [Test]
        public void CorrectRecievedLinksReturnedForUser1()
        {

            Runner.RunScenario(
                given => LinksAreRequested(),

                when => GetEndpoint($"/libraries/{user1Id}/links/received").As(user1Id),

                then => ResponseIs(
                    new LibrarySearchResult(user5Id, "library5", "user5Picture")
                )
            );
        }

        [Test]
        public void CorrectRecievedLinksReturnedForUser2()
        {

            Runner.RunScenario(
                given => LinksAreRequested(),

                when => GetEndpoint($"/libraries/{user2Id}/links/received").As(user2Id),

                then => ResponseIs(
                    new LibrarySearchResult(user1Id, "library1", "user1Picture"),
                    new LibrarySearchResult(user5Id, "library5", "user5Picture")
                )
            );
        }

        [Test]
        public void CorrectRecievedLinksReturnedForUser5()
        {

            Runner.RunScenario(
                given => LinksAreRequested(),

                when => GetEndpoint($"/libraries/{user5Id}/links/received").As(user5Id),

                then => ResponseIs(new LibrarySearchResult[] { })
            );
        }

        [Test]
        public void LinksReturnedForUser1()
        {
            Runner.RunScenario(
                given => LinksAreRequested(),
                and => LinksAreAccepted(),

                when => GetEndpoint($"/libraries/{user1Id}/links/").As(user1Id),

                then => ResponseIs(
                    new LibrarySearchResult(user2Id, "library2", "user2Picture"),
                    new LibrarySearchResult(user3Id, "library3", "user3Picture")
                )
            );
        }

        [Test]
        public void LinksReturnedForUser4()
        {
            Runner.RunScenario(
                given => LinksAreRequested(),
                and => LinksAreAccepted(),

                when => GetEndpoint($"/libraries/{user4Id}/links/").As(user4Id),

                then => ResponseIs(new LibrarySearchResult[] { })
            );
        }

        [Test]
        public void LinksReturnedForUser5()
        {
            Runner.RunScenario(
                given => LinksAreRequested(),
                and => LinksAreAccepted(),

                when => GetEndpoint($"/libraries/{user5Id}/links/").As(user5Id),

                then => ResponseIs(
                    new LibrarySearchResult(user2Id, "library2", "user2Picture")
                )
            );
        }

    }
}
