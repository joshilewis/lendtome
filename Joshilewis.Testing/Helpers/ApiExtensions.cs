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

        private static PostCallBuilder whenPostCallBuilder;
        public static PostCallBuilder WhenCommand(AuthenticatedCommand command)
        {
            whenPostCallBuilder = new PostCallBuilder(client, GetTokeniser(), command, false);
            return whenPostCallBuilder;
        }

        private static GetCallBuilder whenGetCallBuilder;

        public static GetCallBuilder GetEndpoint(string uri)
        {
            whenGetCallBuilder = new GetCallBuilder(client, GetTokeniser(), uri);
            return whenGetCallBuilder;
        }

        public static void ResponseIs<TPayload>(params TPayload[] expected)
        {
            whenGetCallBuilder.Returns(expected);
        }

    }
}