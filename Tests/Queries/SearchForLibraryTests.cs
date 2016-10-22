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
        private Guid userId;
        private Guid user2Id;
        private Guid user3Id;
        private Guid user4Id;

        public override void SetUp()
        {
            base.SetUp();
            transactionId = Guid.Empty;
            userId = Guid.NewGuid();
            user2Id = Guid.NewGuid();
            user3Id = Guid.NewGuid();
            user4Id = Guid.NewGuid();
        }

        [Test]
        public void SearchingForLibraryWithSingleMatchShouldReturnThatUser()
        {
            Runner.RunScenario(
                given => UsersRegistered(),
                and => LibrariesOpened(),

                when => SearchForLibraries("Lew"),
                then => ResponseIs(new LibrarySearchResult(userId, "Joshua Lewis", "user1Picture")));
        }

        [Test]
        public void SearchingForLibraryWithSingleMatchWithWrongCaseShouldReturnThatUser()
        {
            Runner.RunScenario(
                given => UsersRegistered(),
                and => LibrariesOpened(),

                when => SearchForLibraries("lEw"),
                then => ResponseIs(new LibrarySearchResult(userId, "Joshua Lewis", "user1Picture")));
        }

        [Test]
        public void SearchingForLibraryWithNoMatchesShouldReturnEmptyList()
        {
            Runner.RunScenario(
                given => UsersRegistered(),
                and => LibrariesOpened(),

                when => SearchForLibraries("Pet"),
                then => ResponseIs(new LibrarySearchResult[] {}));
        }

        [Test]
        public void SearchingForLibraryWithTwoMatchsShouldReturnTwoLibraries()
        {
            Runner.RunScenario(
                given => UsersRegistered(),
                and => LibrariesOpened(),

                when => SearchForLibraries("Jos"),
                then => ResponseIs(
                    new LibrarySearchResult(userId, "Joshua Lewis", "user1Picture"),
                    new LibrarySearchResult(user3Id, "Josie Doe", "user3Picture")));
        }

        [Test]
        public void SearchingForLibraryThatMatchesSelfShouldExcludeSelfFromResults()
        {
            Runner.RunScenario(
                given => UsersRegistered(),
                and => LibrariesOpened(),

                when => SearchForLibrariesAsUser("Jos", userId),
                then => ResponseIs(new LibrarySearchResult(user3Id, "Josie Doe", "user3Picture")));
        }

        private void UsersRegistered()
        {
            UserRegisters(userId, "user1", "email1", "user1Picture");
            UserRegisters(user2Id, "user2", "email2", "user2Picture");
            UserRegisters(user3Id, "user3", "email3", "user3Picture");
            UserRegisters(user4Id, "user4", "email4", "user4Picture");
        }

        private void LibrariesOpened()
        {
            LibraryOpened(transactionId, userId, "Joshua Lewis");
            LibraryOpened(transactionId, user2Id, "Suzaan Hepburn");
            LibraryOpened(transactionId, user3Id, "Josie Doe");
            LibraryOpened(transactionId, user4Id, "Audrey Hepburn");
        }

    }
}
