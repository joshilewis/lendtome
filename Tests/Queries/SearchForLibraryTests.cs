using System;
using Lending.ReadModels.Relational.ListLibrayLinks;
using NUnit.Framework;
using static Joshilewis.Testing.Helpers.ApiExtensions;
using static Tests.AutomationExtensions;

namespace Tests.Queries
{
    [TestFixture]
    public class SearchForLibraryTests: Fixture
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
            Given(() => UsersRegistered());
            Given(() => LibrariesOpened());
            When(() => SearchForLibraries("Lew"));
            ThenResponseIs(new LibrarySearchResult(userId, "Joshua Lewis", "user1Picture"));
        }

        [Test]
        public void SearchingForLibraryWithSingleMatchWithWrongCaseShouldReturnThatUser()
        {
            Given(() => UsersRegistered());
            Given(() => LibrariesOpened());
            When(() => SearchForLibraries("lEw"));
            ThenResponseIs(new LibrarySearchResult(userId, "Joshua Lewis", "user1Picture"));
        }

        [Test]
        public void SearchingForLibraryWithNoMatchesShouldReturnEmptyList()
        {
            Given(() => UsersRegistered());
            Given(() => LibrariesOpened());
            When(() => SearchForLibraries("Pet"));
            ThenResponseIs(new LibrarySearchResult[] {});

        }

        [Test]
        public void SearchingForLibraryWithTwoMatchsShouldReturnTwoLibraries()
        {
            Given(() => UsersRegistered());
            Given(() => LibrariesOpened());
            WhenGetEndpoint("libraries/Jos");
            ThenResponseIs(new LibrarySearchResult(userId, "Joshua Lewis", "user1Picture"),
                new LibrarySearchResult(user3Id, "Josie Doe", "user3Picture"));
        }

        [Test]
        public void SearchingForLibraryThatMatchesSelfShouldExcludeSelfFromResults()
        {
            Given(() => UsersRegistered());
            Given(() => LibrariesOpened());
            When(() => SearchForLibraries("Lew"));
            WhenGetEndpoint("libraries/Jos").As(userId);
            ThenResponseIs(new LibrarySearchResult(user3Id, "Josie Doe", "user3Picture"));
        }

        private void UsersRegistered()
        {
            Given(() => UserRegisters(userId, "user1", "email1", "user1Picture"));
            Given(() => UserRegisters(user2Id, "user2", "email2", "user2Picture"));
            Given(() => UserRegisters(user3Id, "user3", "email3", "user3Picture"));
            Given(() => UserRegisters(user4Id, "user4", "email4", "user4Picture"));
        }

        private void LibrariesOpened()
        {
            Given(() => LibraryOpened(transactionId, userId, "Joshua Lewis"));
            Given(() => LibraryOpened(transactionId, user2Id, "Suzaan Hepburn"));
            Given(() => LibraryOpened(transactionId, user3Id, "Josie Doe"));
            Given(() => LibraryOpened(transactionId, user4Id, "Audrey Hepburn"));
        }

    }
}
