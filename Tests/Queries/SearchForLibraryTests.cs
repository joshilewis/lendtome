using System;
using Lending.ReadModels.Relational.ListLibrayLinks;
using NUnit.Framework;
using static Joshilewis.Testing.Helpers.ApiExtensions;
using static Tests.AutomationExtensions;

namespace Tests.Queries
{
    [TestFixture]
    public class SearchForLibraryTests : Fixture
    {
        private Guid transactionId;
        private string user1Id = "user1";
        private string user2Id = "user2";
        private string user3Id = "user3";
        private string user4Id = "user4";
        private Guid library1Id;
        private Guid library2Id;
        private Guid library3Id;
        private Guid library4Id;

        public override void SetUp()
        {
            base.SetUp();
            transactionId = Guid.Empty;
            library1Id = Guid.NewGuid();
            library2Id = Guid.NewGuid();
            library3Id = Guid.NewGuid();
            library4Id = Guid.NewGuid();
        }

        [Test]
        public void SearchingForLibraryWithSingleMatchShouldReturnThatUser()
        {
            Runner.RunScenario(
                given => LibrariesOpened(),

                when => SearchForLibraries("Lew"),
                then => ResponseIs(new LibrarySearchResult(library1Id, "Joshua Lewis", "library1Picture")));
        }

        [Test]
        public void SearchingForLibraryWithSingleMatchWithWrongCaseShouldReturnThatUser()
        {
            Runner.RunScenario(
                given => LibrariesOpened(),

                when => SearchForLibraries("lEw"),
                then => ResponseIs(new LibrarySearchResult(library1Id, "Joshua Lewis", "library1Picture")));
        }

        [Test]
        public void SearchingForLibraryWithNoMatchesShouldReturnEmptyList()
        {
            Runner.RunScenario(
                given => LibrariesOpened(),

                when => SearchForLibraries("Pet"),
                then => ResponseIs(new LibrarySearchResult[] { }));
        }

        [Test]
        public void SearchingForLibraryWithTwoMatchsShouldReturnTwoLibraries()
        {
            Runner.RunScenario(
                given => LibrariesOpened(),

                when => SearchForLibraries("Jos"),
                then => ResponseIs(
                    new LibrarySearchResult(library1Id, "Joshua Lewis", "library1Picture"),
                    new LibrarySearchResult(library3Id, "Josie Doe", "library3Picture")));
        }

        [Test]
        public void SearchingForLibraryThatMatchesSelfShouldExcludeSelfFromResults()
        {
            Runner.RunScenario(
                given => LibrariesOpened(),

                when => SearchForLibrariesAsUser("Jos", user1Id),
                then => ResponseIs(new LibrarySearchResult(library3Id, "Josie Doe", "library3Picture")));
        }

        private void LibrariesOpened()
        {
            LibraryOpened(transactionId, user1Id, library1Id, "Joshua Lewis", "library1Picture");
            LibraryOpened(transactionId, user2Id, library2Id, "Suzaan Hepburn", "library2Picture");
            LibraryOpened(transactionId, user3Id, library3Id, "Josie Doe", "library3Picture");
            LibraryOpened(transactionId, user4Id, library4Id, "Audrey Hepburn", "library4Picture");
        }

    }
}
