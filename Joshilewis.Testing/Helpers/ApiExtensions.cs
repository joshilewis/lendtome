using System;
using System.Net;
using System.Net.Http;
using Joshilewis.Cqrs.Command;
using Joshilewis.Infrastructure.Auth;
using Joshilewis.Testing.CallBuilders;
using Microsoft.Owin.Testing;

namespace Joshilewis.Testing.Helpers
{
    public static class ApiExtensions
    {
        private static TestServer server;
        private static HttpClient client;

        public static void SetUpOwinServer<TStartup>()
        {
            server = TestServer.Create<TStartup>();
            client = server.HttpClient;
        }

        public static void TearDownOwinServer()
        {
            server.Dispose();
        }

        private static Tokeniser GetTokeniser()
        {
            return DIExtensions.Container.GetInstance<Tokeniser>();
        }

        public static PostCallBuilder GivenCommand(AuthenticatedCommand command)
        {
            return new PostCallBuilder(client, GetTokeniser(), command, true);
        }

        public static MultiPostCallBuilder GivenCommands(params AuthenticatedCommand[] commands)
        {
            return new MultiPostCallBuilder(client, GetTokeniser(), commands);
        }

        private static PostCallBuilder whenPostCallBuilder;
        public static PostCallBuilder WhenCommand(AuthenticatedCommand command)
        {
            whenPostCallBuilder = new PostCallBuilder(client, GetTokeniser(), command, false);
            return whenPostCallBuilder;
        }

        public static void Then(HttpResponseMessage expectedResponseMessage)
        {
            whenPostCallBuilder.Response.ShouldEqual(expectedResponseMessage);
        }

        public static HttpResponseMessage Http403BecauseUnauthorized(Guid userId, Guid aggregateId, Type aggregateType)
        {
            return new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                ReasonPhrase = $"User {userId} is not authorized for {aggregateType} {aggregateId}"
            };
        }

        private static GetCallBuilder whenGetCallBuilder;

        public static GetCallBuilder WhenGetEndpoint(string uri)
        {
            whenGetCallBuilder = new GetCallBuilder(client, GetTokeniser(), uri);
            return whenGetCallBuilder;
        }

        public static void ThenResponseIs<TPayload>(params TPayload[] expected)
        {
            whenGetCallBuilder.Returns(expected);
        }

        public static GetCallBuilder AndGETTo(string url)
        {
            return new GetCallBuilder(client, GetTokeniser(), url);
        }

        public static HttpResponseMessage Http400Because(string reasonPhrase)
        {

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                ReasonPhrase = reasonPhrase
            };
        }
    }
}