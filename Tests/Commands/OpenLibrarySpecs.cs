using System;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using NUnit.Framework;
using static Joshilewis.Testing.Helpers.EventStoreExtensions;
using static Tests.AutomationExtensions;

namespace Tests.Commands
{
    [TestFixture]
    public class OpenLibrarySpecs : Fixture
    {
        [Test]
        public void UserCanOpenALibrary()
        {
            //Given
            var transactionId = Guid.Empty;
            var userId = "userId";
            Guid libraryId = Guid.NewGuid();
            //Given
            UserRegisters(userId, "user1", "email1", "user1Picture");
            //When
            libraryId = OpenLibrary(transactionId, userId, "library1");
            //Then
            LibraryOpenedSuccessfully();
                EventsSavedForAggregate<Library>(libraryId,
                    new LibraryOpened(transactionId, libraryId, "library1", userId));
        }

        [Test]
        public void UserCanOpenASecondLibrary()
        {
            var transactionId = Guid.Empty;
            var userId = "userId";
            Guid libraryId = Guid.Empty;
            //Given
            UserRegisters(userId, "user1", "email1", "user1Picture");
            libraryId = OpenLibrary(transactionId, userId, "library1");
            //When
            Guid libraryId2 = OpenLibrary(transactionId, userId, "library1");
            //Then
            LibraryOpenedSuccessfully();
            EventsSavedForAggregate<Library>(libraryId, 
                new LibraryOpened(transactionId, libraryId, "library1", userId));
            EventsSavedForAggregate<Library>(libraryId2,
                new LibraryOpened(transactionId, libraryId2, "library1", userId));
        }


    }
}
