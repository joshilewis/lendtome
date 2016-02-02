using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Lending.Cqrs.Command;
using Lending.Execution.Auth;

namespace Tests
{
    public class CallBuilder
    {
        public AuthenticatedCommand Command { get; }
        public HttpResponseMessage Response { get; private set; }
        public Exception Exception { get; private set; }
        private readonly HttpClient client;
        private readonly Tokeniser tokeniser;
        public string Url { get; private set; }

        public CallBuilder(AuthenticatedCommand command, HttpClient client, Tokeniser tokeniser)
        {
            Command = command;
            this.client = client;
            this.tokeniser = tokeniser;
        }

        public void IsPOSTedTo(string url)
        {
            Url = url;
            try
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokeniser.CreateToken("username", Command.UserId));
                Response = client.PostAsJsonAsync($"https://localhost/api/{url}", Command).Result;
            }
            catch (Exception exception)
            {
                Exception = exception;
            }
        }

        public void IsPUTedTo(string url)
        {
            try
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(tokeniser.CreateToken("username", Command.UserId));
                Response = client.PutAsJsonAsync($"https://localhost/api/{url}", Command).Result;
            }
            catch (Exception exception)
            {
                Exception = exception;
            }
        }
    }
}