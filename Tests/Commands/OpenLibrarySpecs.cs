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
            //When
            libraryId = OpenLibrary(transactionId, userId, "library1", "library1Picture");
            //Then
            LibraryOpenedSuccessfully();
                EventsSavedForAggregate<Library>(libraryId,
                    new LibraryOpened(transactionId, libraryId, "library1", userId, "library1Picture"));
        }

        [Test]
        public void UserCanOpenASecondLibrary()
        {
            var transactionId = Guid.Empty;
            var userId = "userId";
            Guid libraryId = Guid.Empty;
            //Given
            libraryId = OpenLibrary(transactionId, userId, "library1", "library1Picture");
            //When
            Guid libraryId2 = OpenLibrary(transactionId, userId, "library1", "library1Picture");
            //Then
            LibraryOpenedSuccessfully();
            EventsSavedForAggregate<Library>(libraryId, 
                new LibraryOpened(transactionId, libraryId, "library1", userId, "library1Picture"));
            EventsSavedForAggregate<Library>(libraryId2,
                new LibraryOpened(transactionId, libraryId2, "library1", userId, "library1Picture"));
        }


    }
}
