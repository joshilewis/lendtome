using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Testing;
using Joshilewis.Testing.CallBuilders;
using Joshilewis.Testing.Helpers;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.ReadModels.Relational;
using static Joshilewis.Testing.Helpers.ApiExtensions;


namespace Tests
{
    public static class AutomationExtensions
    {
        private static PostCallBuilder command;

        public static void UserRegisters(Guid id, string name, string emailAddress, string picture)
        {
            PersistenceExtensions.OpenTransaction();
            PersistenceExtensions.Repository.Save(new AuthenticatedUser(id, name, emailAddress, picture, new List<AuthenticationProvider>()));
            PersistenceExtensions.CommitTransaction();
        }

        public static void OpenLibrary(Guid processId, Guid userId, string name)
        {
            command = WhenCommand(new OpenLibrary(processId, userId, userId, name));
            command.IsPOSTedTo("/libraries");
        }

        public static void LibraryCreatedSuccessfully()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.Created));
        }

        public static void SecondLibraryNotCreated()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                ReasonPhrase = LibraryOpener.UserAlreadyOpenedLibrary,
            });
        }

        public static void AddBookToLibrary1(Guid transactionId, Guid libraryId, Guid userId, string title, string author,
            string isbn, int publishYear)
        {
            command =
                WhenCommand(new AddBookToLibrary(transactionId, libraryId, userId, title, author, isbn,
                    publishYear));
            command.IsPOSTedTo($"/libraries/{libraryId}/books/add");
        }

        public static void RemoveBookFromLibrary(Guid transactionId, Guid libraryId, Guid userId, string title, string author,
            string isbn, int publishYear)
        {
            command =
                WhenCommand(new RemoveBookFromLibrary(transactionId, libraryId, userId, title, author, isbn,
                    publishYear));
            command.IsPOSTedTo($"/libraries/{libraryId}/books/remove");
        }

        public static void BookAddedSucccessfully()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.Created));
        }

        public static void DuplicateBookNotAdded()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                ReasonPhrase = Library.BookAlreadyInLibrary,
            });
        }

        public static void UnauthorisedCommandRejected(Guid userId, Type aggregateType, Guid aggregateId)
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                ReasonPhrase = $"User {userId} is not authorized for {aggregateType} {aggregateId}"
            });
        }

        public static void BookRemovedSucccessfully()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public static void BookNotInLibrary()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                ReasonPhrase = Library.BookNotInLibrary
            });
        }

    }
}
