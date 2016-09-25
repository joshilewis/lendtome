using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Testing;
using Joshilewis.Testing.CallBuilders;
using Lending.Domain.OpenLibrary;
using static Joshilewis.Testing.Helpers.ApiExtensions;


namespace Tests
{
    public static class AutomationExtensions
    {
        private static PostCallBuilder command;

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
    }
}
