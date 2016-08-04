using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Joshilewis.Cqrs.Command;
using Joshilewis.Infrastructure.Auth;
using NUnit.Framework;

namespace Joshilewis.Testing.CallBuilders
{
    public class PostCallBuilder : CallBuilder
    {
        public AuthenticatedCommand Command { get; }
        public HttpResponseMessage Response { get; private set; }
        public string Url { get; private set; }
        private readonly bool failIfUnsuccessful;

        public PostCallBuilder(HttpClient client, Tokeniser tokeniser, AuthenticatedCommand command, bool failIfUnsuccessful)
            : base(client, tokeniser)
        {
            Command = command;
            this.failIfUnsuccessful = failIfUnsuccessful;
        }

        public void IsPOSTedTo(string url)
        {
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(Tokeniser.CreateToken("username", Command.UserId));
            Url = $"https://localhost/api/{url}";
            Response = Client.PostAsJsonAsync(Url, Command).Result;
            Console.WriteLine("POST to {0} with body {1} returned status {2} with reason {3}", url, Serialize(new JsonMediaTypeFormatter(), Command), Response.StatusCode, Response.ReasonPhrase);
            if (failIfUnsuccessful && !Response.IsSuccessStatusCode)
                throw new AssertionException(
                    $"POST call to '{Url}' was not successful, response code is {Response.StatusCode}, reason {Response.ReasonPhrase}");

        }

    }
}