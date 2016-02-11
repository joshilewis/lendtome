using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Lending.Cqrs.Command;
using Lending.Execution.Auth;
using NUnit.Framework;

namespace Tests
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
            failIfUnsuccessful = failIfUnsuccessful;
        }

        public void IsPOSTedTo(string url)
        {
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(Tokeniser.CreateToken("username", Command.UserId));
            Url = $"https://localhost/api/{url}";
            Response = Client.PostAsJsonAsync(Url, Command).Result;
            if (failIfUnsuccessful && !Response.IsSuccessStatusCode)
                throw new AssertionException(
                    $"POST call to '{Url}' was not successful, response code is {Response.StatusCode}, reason {Response.ReasonPhrase}");

        }
    }
}