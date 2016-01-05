using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs.Query;
using Lending.Domain.AcceptConnection;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.Model;
using Lending.Domain.RegisterUser;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.Domain.RequestConnection;
using Lending.Execution.Auth;

namespace Tests
{
    public class DefaultTestData
    {
        public static RegisteredUser RegisteredUser1 => new RegisteredUser(1, Guid.Empty, "Joshua Lewis");

        public static RegisteredUser RegisteredUser2 => new RegisteredUser(2, Guid.Empty, "User2");

        public static RegisteredUser RegisteredUser3 => new RegisteredUser(3, Guid.Empty, "User3");

        public static Guid processId = Guid.Empty;
        public static Guid user1Id = Guid.NewGuid();
        public static Guid user2Id = Guid.NewGuid();
        public static Guid user3Id = Guid.NewGuid();
        public static Guid user4Id = Guid.NewGuid();
        public static Guid user5Id = Guid.NewGuid();
        public static Guid user6Id = Guid.NewGuid();

        public static RegisterUser user1Registers = new RegisterUser(processId, user1Id, 1, "user1", "email1");

        public static UserRegistered user1Registered = new UserRegistered(processId, user1Id, 1, user1Registers.UserName,
            user1Registers.PrimaryEmail);

        public static string title = "title";
        public static string author = "author";
        public static string isbnnumber = "isbn";

        public static AddBookToLibrary user1AddsBookToLibrary = new AddBookToLibrary(processId, user1Id, user1Id, title,
            author, isbnnumber);

        public static BookAddedToLibrary book1AddedToUser1Library = new BookAddedToLibrary(processId, user1Id, title,
            author, isbnnumber);

        public static RemoveBookFromLibrary user1RemovesBookFromLibrary = new RemoveBookFromLibrary(processId, user1Id,
            user1Id, title, author, isbnnumber);

        public static BookRemovedFromLibrary book1RemovedFromLibrary = new BookRemovedFromLibrary(processId, user1Id,
            title, author, isbnnumber);

        public static Result succeed = new Result();
        public static Result failBecauseBookAlreadyInLibrary = new Result(User.BookAlreadyInLibrary);
        public static Result failBecauseBookNotInLibrary = new Result(User.BookNotInLibrary);
        public static Result failBecauseNoPendingConnectionRequest = new Result(User.NoPendingConnectionRequest);
        public static Result failBecauseUsersAlreadyConnected = new Result(User.UsersAlreadyConnected);
        public static RegisterUser user2Registers = new RegisterUser(processId, user2Id, 2, "user2", "email2");

        public static RequestConnection user1RequestsConnectionToUser2 = new RequestConnection(processId, user1Id,
            user1Id, user2Id);

        public static ConnectionRequested connectionRequestedFrom1To2 = new ConnectionRequested(processId, user1Id,
            user2Id);

        public static UserRegistered user2Registered = new UserRegistered(processId, user2Id, 2, user2Registers.UserName,
            user2Registers.PrimaryEmail);

        public static ConnectionRequestReceived connectionRequestFrom1To2Received =
            new ConnectionRequestReceived(processId, user2Id, user1Id);

        public static AcceptConnection user2AcceptsRequestFrom1 = new AcceptConnection(processId, user2Id, user2Id,
            user1Id);

        public static ConnectionCompleted connectionCompleted = new ConnectionCompleted(processId, user1Id, user2Id);
        public static ConnectionAccepted connectionAccepted = new ConnectionAccepted(processId, user2Id, user1Id);
        public static Result failBecauseConnectionAlreadyRequested = new Result(User.ConnectionAlreadyRequested);

        public static Result failBecauseTargetUserDoesNotExist =
            new Result(RequestConnectionHandler.TargetUserDoesNotExist);

        public static Result failBecauseReverseConnectionAlreadyRequested =
            new Result(User.ReverseConnectionAlreadyRequested);

        public static Result failBecauseCantConnectToSelf = new Result(RequestConnectionHandler.CantConnectToSelf);

        public static RequestConnection user2RequestsConnectionToUser1 = new RequestConnection(processId, user2Id,
            user2Id, user1Id);

        public static RequestConnection user1Requests2ndConnectionToUser2 = new RequestConnection(processId, user1Id,
            user1Id, user2Id);

        public static ConnectionRequested connectionRequestedFrom2To1 = new ConnectionRequested(processId, user2Id,
            user1Id);

        public static ConnectionRequestReceived connectionRequestFrom2To1Received =
            new ConnectionRequestReceived(processId, user1Id, user2Id);

        public static RequestConnection user1RequestsConnectionToSelf = new RequestConnection(processId, user1Id,
            user1Id, user1Id);

        public static AcceptConnection user2AcceptsRequest1 = new AcceptConnection(processId, user2Id, user2Id, user1Id);

        public static UserRegistered joshuaLewisRegistered = new UserRegistered(processId, user1Id, 1, "Joshua Lewis",
            "Email1");

        public static UserRegistered suzaanHepburnRegistered = new UserRegistered(processId, user2Id, 2,
            "Suzaan Hepburn", "Email2");

        public static UserRegistered josieDoe3Registered = new UserRegistered(processId, user3Id, 3, "Josie Doe",
            "Email3");

        public static UserRegistered audreyHepburn4Registered = new UserRegistered(processId, user4Id, 4,
            "Audrey Hepburn", "Email4");

        public static string TestDrivenDevelopment = "Test-Driven Development";
        public static string KentBeck = "Kent Beck";
        public static string Isbn = "Isbn";
        public static string ExtremeProgrammingExplained = "Extreme Programming Explained";
        public static string ExtremeSnowboardStunts = "Extreme Snowboard Stunts";
        public static string SomeSkiier = "Some Skiier";
        public static string BeckAMusicalMaestro = "Beck: A musical Maestro";
        public static string SomeAuthor = "Some Author";

        public static UserRegistered user3Registered = new UserRegistered(processId, user3Id, 3, "User3", "Email3");
        public static UserRegistered user4Registered = new UserRegistered(processId, user4Id, 4, "User4", "Email4");
        public static UserRegistered user5Registered = new UserRegistered(processId, user5Id, 5, "User5", "Email5");
        public static UserRegistered user6Registered = new UserRegistered(processId, user6Id, 6, "User6", "Email6");

        public static ConnectionAccepted conn1To2Accepted = new ConnectionAccepted(processId, user2Id, user1Id);
        public static ConnectionAccepted conn1To3Accepted = new ConnectionAccepted(processId, user3Id, user1Id);
        public static ConnectionAccepted conn1To4Accepted = new ConnectionAccepted(processId, user4Id, user1Id);
        public static ConnectionAccepted conn1To5Accepted = new ConnectionAccepted(processId, user5Id, user1Id);
        public static ConnectionAccepted conn1To6Accepted = new ConnectionAccepted(processId, user6Id, user1Id);

        public static BookAddedToLibrary xpeByKbAddTo2 = new BookAddedToLibrary(processId, user2Id,
            ExtremeProgrammingExplained, KentBeck, Isbn);

        public static BookAddedToLibrary xpeByKbAddTo4 = new BookAddedToLibrary(processId, user4Id,
            ExtremeProgrammingExplained, KentBeck, Isbn);

        public static BookAddedToLibrary tddByKbAddTo3 = new BookAddedToLibrary(processId, user3Id,
            TestDrivenDevelopment, KentBeck, Isbn);

        public static BookAddedToLibrary essBySSAddTo5 = new BookAddedToLibrary(processId, user5Id,
            ExtremeSnowboardStunts, SomeSkiier, Isbn);

        public static BookAddedToLibrary bBySAAddTo6 = new BookAddedToLibrary(processId, user6Id, BeckAMusicalMaestro,
            SomeAuthor, Isbn);

        public static BookRemovedFromLibrary xpeByKbRemoveFrom4 = new BookRemovedFromLibrary(processId, user4Id,
            ExtremeProgrammingExplained, KentBeck, Isbn);

    }
}
